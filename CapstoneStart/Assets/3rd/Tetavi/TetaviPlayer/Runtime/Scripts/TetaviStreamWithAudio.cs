using System;
using System.Linq;
using static TetaviCAPI;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEditor;

public class TetaviStreamCompositeWithAudio : TetaviStreamComposite
{
    AudioSource audioPlayer;
    private int audioDelayAllowed = 5; // seek to sync when more than 5 frames diff between audio and frames
    private int audioDelayPausing = 15; // if in pause, pause the audio after some delay to avoid on/off when buffering

    public TetaviStreamCompositeWithAudio(bool rendering, AudioSource audioPlayer) 
        : base(rendering)
    {
        this.audioPlayer = audioPlayer;
    }

    protected override TetaviStreamFile CreateStreamFile(bool rendering) => new TetaviStreamChunkAudio(rendering, audioPlayer);

    public override bool HasAudio()
    {
        bool result = hasAudio;
        if (comps.Count == 1 && comps.First().Value.IsOpen())
        {
            if (comps.First().Value is TetaviStreamChunkAudio compsAudio)
            {
                result = compsAudio.HasAudio();
            }
        }
        return result;
    }

    public override TetaviStreamChunk CreateStreamChunk(bool r, string n, string f, string u, int f1, int f2) => new TetaviStreamChunkAudio(r, n, f, u, f1, f2, audioPlayer);

    public override IntPtr GetFrameObj(int frameIdx, out int sid)
    {
        IntPtr res = base.GetFrameObj(frameIdx, out sid);
        foreach (var c in comps)
            (c.Value as TetaviStreamChunkAudio).CheckAudioLoaded();
        return res;
    }

    public override bool AudioSync(int framePlaying, bool pause, ref bool audioPosDirty, ref int audioFrameRel)
    {
        if (!audioPlayer)
            return false;
        TetaviStreamFile subClip = GetSubClip(framePlaying);
        (subClip as TetaviStreamChunkAudio).SetAudioClip();

        int frameRel = subClip.GetFrameOfSubClip(framePlaying);
        audioFrameRel = (int)(audioPlayer.time * GetFps());
        if (!audioPlayer.isPlaying || audioPosDirty || Math.Abs(audioFrameRel - frameRel) > audioDelayAllowed)
        {
            if (pause) // if in pause, pause the audio with delay 
            {
                if (audioPlayer.isPlaying && Math.Abs(audioFrameRel - frameRel) > audioDelayPausing)
                {
                    Debug.Log($"Frame paused for {audioFrameRel - frameRel} frames => Pause audio");
                    audioPlayer.Pause();
                }
            }
            else if (!audioPosDirty && audioPlayer.isPlaying) // if playing adjust frame-time to audio-time
            {
                Debug.LogWarning($"Frame adjusted by Audio time by {audioFrameRel - frameRel} frames");
                return true; // SetFramePlaying(audioFrameRel);
            }
            else // adjust audio-time to frame-time
            {
                Debug.LogWarning($"Audio time adjusted by {audioFrameRel - frameRel} frames");
                audioPlayer.time = ((float)frameRel / GetFps());
                audioPlayer.Play(); // redundant if already playing but no artifact
                audioPosDirty = false;
            }
        }
        return false;
    }
}

public class TetaviStreamChunkAudio : TetaviStreamChunk
{
    AudioReader audioReader;
    AudioSource audioPlayer;
    AudioClip audioClip;
    UInt32 mAudioSampleRate = 0;
    UInt32 bitsPerSample = 0;
    UInt32 mAudioChannels = 0;
    long decodedBytesNumber = -1;
    long samplesNumber = -1;
    private int mAudioLengthSamples = 0;
    bool audioNeedsToBeLoaded = false;

    public SOURCE_TYPE sourceType = SOURCE_TYPE.PLAYBACK;

    public enum SOURCE_TYPE
    {
        PLAYBACK = 0,
        RTMP = 2,
    }

    public TetaviStreamChunkAudio(bool rendering, AudioSource audioPlayer)   // constructed for non streaming file
        : base(rendering)
    {
        this.audioPlayer = audioPlayer;
    }

    public TetaviStreamChunkAudio(bool r, string n, string f, string u, int f1, int f2, AudioSource audioPlayer)    // constructed as an actual "chunk"
        : base(r, n, f, u, f1, f2)
    {
        this.audioPlayer = audioPlayer;
    }

    override public void Close()
    {
        base.Close();

        if (audioReader != null)
        {
            audioReader.Release();
        }
    }

    public override bool HasAudio()
    {
        return stream_has_audio(stream);
    }

    public override void Open(string filename)
    {
        base.Open(filename);
        audioNeedsToBeLoaded = HasAudio();// && audioPlayer != null;
        if (likeBase)
        {
            CheckAudioLoaded();
            if (audioClip != null && audioPlayer != null)
                audioPlayer.clip = audioClip;
        }
    }

    public void CheckAudioLoaded ()
    { 
        if (audioNeedsToBeLoaded && IsOpen() && audioClip == null && audioPlayer != null)
        {
            audioNeedsToBeLoaded = false;
            audioReader = AudioReader.CreateAudioReader(stream);
            audioReader.GetCharacteristics(ref mAudioSampleRate, ref bitsPerSample, ref mAudioChannels, ref mAudioLengthSamples);
            audioReader.GetDecodedBytesAndSamplesCount(ref decodedBytesNumber, ref samplesNumber);
            int lengthSamples = (int)(decodedBytesNumber) / 2;
            audioClip = AudioClip.Create("AudioVoice", lengthSamples, (int)mAudioChannels, (int)(mAudioSampleRate), false);
            int nSamples = 0;
            for (int i = 0; i < samplesNumber; i++)
            {
                byte[] data = null;
                if (audioReader.GetAudioByteData(i, ref data) && data != null)
                {
                    float[] pcmData = AudioReader.Convert16BitByteArrayToAudioClipData(data);
                    if (pcmData.Length > 0)
                    {
                        audioClip.SetData(pcmData, nSamples);
                        nSamples += pcmData.Length / (int)mAudioChannels;
                    }
                }
            }
        }
    }

    public void SetAudioClip ()
    {
        audioPlayer.clip = audioClip;
    }
}

class AudioReaderData
{
    public byte[] audio_bytes;
    public GCHandle gcHandlerAudioBytes;
    ~AudioReaderData ()
    {
        ClearAudioBuffer();
    }
    public void AllocAudioBuffers(int count)
    {
        ClearAudioBuffer();
        audio_bytes = new byte[count];
        gcHandlerAudioBytes = GCHandle.Alloc(audio_bytes, GCHandleType.Pinned);
    }
    public void ClearAudioBuffer()
    {
        if (gcHandlerAudioBytes.IsAllocated) 
            gcHandlerAudioBytes.Free();
        audio_bytes = null;
    }
}

public class AudioReader
{
    public IntPtr audioPlayer { get; private set; } = IntPtr.Zero;
    AudioInfo info;
    AudioReaderData audioData = null;

    public void GetCharacteristics(ref UInt32 bytesPerSecond, ref UInt32 bitsPerSample, ref UInt32 chanelNum, ref int lengthSamples)
    {
        bytesPerSecond = info.bytes_per_second;
        bitsPerSample = info.bits_per_sample;
        chanelNum = info.chanel_num;
    }

    public void GetDecodedBytesAndSamplesCount(ref long decodedBytesNumber, ref long samplesNumber)
    {
        decodedBytesNumber = info.decoded_bytes_number;
        samplesNumber = info.samples_number;
    }

    static public AudioReader CreateAudioReader(IntPtr native_stream_ptr)
    {
        return new AudioReader(native_stream_ptr);
    }

    private AudioReader(IntPtr native_stream_ptr)
    {
        audioData = new AudioReaderData();
        Debug.Log("com." + Application.companyName + Application.productName);
        audioPlayer = create_audio_obj(native_stream_ptr, "com." + Application.companyName + "." + Application.productName);
        info.block_align = 0;
        info = audio_player_get_info(audioPlayer);
    }

    ~AudioReader()
    {
        Release();
    }

    internal void Release()
    {
        audioData.ClearAudioBuffer();
        IntPtr audioPlayer = this.audioPlayer;
        this.audioPlayer = IntPtr.Zero; // atomic operation to sign it as non-initiated, for thread safety. 
        if (audioPlayer != IntPtr.Zero) // if Init was checked and issued mean while, another stream is opened on the same tet, but this one is closed.
            audio_player_release(audioPlayer);
    }

    public bool GetAudioByteData(int frame_idx, ref byte[] data)
    {
        bool result = false;
        int byte_count = audio_player_get_audio_buffer_byte_size(audioPlayer, frame_idx);
        data = new byte[byte_count];
        GCHandle gcHandlerAudioBytes = GCHandle.Alloc(data, GCHandleType.Pinned);
        IntPtr data_ptr = gcHandlerAudioBytes.AddrOfPinnedObject();
        result = audio_player_copy_frame(audioPlayer, frame_idx, ref data_ptr);
        if (gcHandlerAudioBytes.IsAllocated) gcHandlerAudioBytes.Free();
        return result;
    }

    public static float[] Convert16BitByteArrayToAudioClipData(byte[] source)
    {
        int x = sizeof(Int16);
        int convertedSize = source.Length / x;
        float[] data = new float[convertedSize];
        for (int i = 0; i < convertedSize; i++)
        {
            int offset = i * x;
            data[i] = (float)BitConverter.ToInt16(source, offset) / Int16.MaxValue;
        }
        return data;
    }
}
