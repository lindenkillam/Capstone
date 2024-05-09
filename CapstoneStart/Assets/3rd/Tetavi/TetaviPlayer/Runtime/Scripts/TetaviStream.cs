//#define EMULATE_NETWORK

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine;
using static TetaviCAPI;
using System.Net;
using System.Linq;

#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.Android;
#endif

public interface TetaviStreamIfc
{
    void Open(string fileNameOrPath);
    void SetCalcNormals(bool calc);
    bool IsOpen();
    void Close();
    bool CanOpen();
    bool IsInRange(int frameIdx);
    int GetFrameOfSubClip(int frameIdx);
    int GetFrameCount();
    IntPtr GetFrameObj(int frameIdx, out int sid);
    int IsBuffering();
    int GetFramesDecodedAhead();
    string GetProfilingInfo();
    string GetMatClassInfo();
    
    void SetTexturesN12Targets(IntPtr texY, IntPtr texUV, int w, int h);
    void SetTexturesYUV420Targets(IntPtr texY, IntPtr texU, IntPtr texV, int w, int h);
    int GetFps();
    int GetWidth();
    int GetHeight();
    bool HasAudio();
    bool AudioSync(int framePlaying, bool pause, ref bool audioPosDirty, ref int audioFrameRel);
}
// ------------------------------------------------------------------------------------------
// base class - without streaming
public class TetaviStreamFile : TetaviStreamIfc
{
    protected IntPtr stream = IntPtr.Zero;
    protected bool rendering;
    protected int frameCount = -1;
    public int width =2048, height=2048;
    public string mcInfo;
    public TetaviStreamFile(bool rendering = false)
    {
        this.rendering = rendering;
    }
    ~TetaviStreamFile()
    {
        Close();
    }
     public virtual void Open(string pathToFile)
    {
        //assert(this.stream==IntPtr.Zero)
        IntPtr stream = rendering ? create_stream_rendering() : create_stream_decoder_obj_YUV();
        stream_init_model(stream, pathToFile);
        this.stream = stream; // atomic operation to sign it initiated, for thread safety
        frameCount = stream_get_frame_count(stream);
        TextureInfo textureInfo = stream_get_video_width_height(stream);
        width = textureInfo.width;
        height = textureInfo.height;
        // test:
        mcInfo = GetMatClassInfo();
        if (mcInfo.Length > 0)
            Debug.Log("Opened tet with Material class info");
    }
    public virtual void SetCalcNormals(bool calc) => stream_set_calculate_normals(stream, calc);
    public virtual void SetCalcTangents(bool calc) => stream_set_calculate_tangents(stream, calc);
    public virtual bool IsOpen () => stream!=IntPtr.Zero;
    public virtual void Close ()
    {
        IntPtr stream = this.stream;
        this.stream = IntPtr.Zero; // atomic operation to sign it as non-initiated, for thread safety. 
        if (stream != IntPtr.Zero) // if Init was checked and issued mean while, another stream is opened on the same tet, but this one is closed.
            stream_release(stream);
    }
    public virtual bool CanOpen() => true; // no streaming: assumme always can open
    public virtual bool IsInRange(int frameIdx) => true; // no streaming: assumme always in range as cyclic is treated within api
    public virtual int GetFrameOfSubClip(int frameIdx) => frameCount<=0 || frameIdx<=0? 0 : frameIdx % frameCount;
    public virtual int GetFrameCount() => frameCount;
    public virtual IntPtr GetFrameObj(int frameIdx, out int sid)
    {
        sid = IsOpen() ? stream_get_id(stream) : 0; // sid needed to be together with frame: in streaming when more than 1 stream is active
        IntPtr frame = IsOpen() ? stream_get_frame_obj(stream, frameIdx) : IntPtr.Zero;
        if (frame!=IntPtr.Zero && rendering)
        {
            stream_apply_rendering_frame(stream, frame);
        }
        return frame;
    }
    public virtual int IsBuffering()
    {
        return IsOpen()? stream_is_buffering(stream) : 1;
    }
    public virtual int GetFramesDecodedAhead()
    {
        return IsOpen() ? stream_frames_decoded_ahead(stream) : -1;
    }
    public virtual string GetProfilingInfo()
    {
        IntPtr pU = IsOpen() ? get_profiling_info(stream) : IntPtr.Zero;
        return  pU == IntPtr.Zero? "" : Marshal.PtrToStringAnsi(pU);
    }
    public virtual string GetMatClassInfo()
    {
        IntPtr pU = IsOpen() ? get_mat_class_info(stream) : IntPtr.Zero;
        return pU == IntPtr.Zero ? "" : Marshal.PtrToStringAnsi(pU);
    }
    public virtual void SetTexturesN12Targets(IntPtr texY, IntPtr texUV, int w, int h)
    {
        stream_set_textures_N12_targets(stream, texY, texUV, w, h);
    }
    public virtual void SetTexturesYUV420Targets(IntPtr texY, IntPtr texU, IntPtr texV, int w, int h)
    {
        stream_set_textures_YUV420_targets(stream, texY, texU, texV, w, h);
    }
    public virtual int GetFps() => IsOpen() ? stream_get_fps(stream) : -1;
    public virtual int GetWidth() => width;
    public virtual int GetHeight() => height;
    public virtual string GetMaterialClassInfo() => mcInfo;
    public virtual bool HasAudio() => false;
    public virtual bool AudioSync(int framePlaying, bool pause, ref bool audioPosDirty, ref int audioFrameRel) => false;
}
// ------------------------------------------------------------------------------------------
// Chunk file used by streaming
public class TetaviStreamChunk : TetaviStreamFile
{
    protected bool likeBase; // For non streaming, TetaviStreamCompositeWithAudio contains a "chunk" that works the same as a single file.
    string filePath;
    public string URL { set; get; }
    bool calcNormals = false;
    long initTime;
    int startFrame, endFramePlusOne;
    bool downloaded;
    const int buffAheadFrames = 150; // 5 sec. todo: use FPS?
    const int buffAheadChunks = 2; // in large chunks frames ahead should be added by 1 (or more?) chunks
    public TetaviStreamChunk(bool rendering, string fileName, string folder, string urlBase, int startFrame, int endFramePlusOne)
        : base(rendering)
    {
        likeBase = false;
        filePath = folder + "/" + fileName;
        URL = urlBase + "/" + fileName;
        this.startFrame = startFrame;
        this.endFramePlusOne = endFramePlusOne;
        var fi = new FileInfo(filePath);
        downloaded = fi.Exists && fi.Length > 0;
    }
    public TetaviStreamChunk(bool rendering)
        : base(rendering)
    {
        likeBase = true;
    }
    public override void Open(string fileName)
    {
        if (likeBase)
        {
            base.Open(fileName);
        }
        else if (CanOpen())
        {
            base.Open(filePath);
            base.SetCalcNormals(calcNormals);
            initTime = DateTime.UtcNow.Ticks;
        }
    }
    public override void SetCalcNormals(bool calc)
    {
        if (likeBase)
        {
            base.SetCalcNormals(calc);
        }
        else
        {
            calcNormals = calc;
            if (IsOpen())
                base.SetCalcNormals(calc);
        }
    }
    public override bool CanOpen() => likeBase ? base.CanOpen() : downloaded;
    public override bool IsInRange(int frameIdx) => likeBase ? base.IsInRange(frameIdx) : frameIdx < endFramePlusOne && frameIdx >= startFrame;
    public override int GetFrameOfSubClip(int frameIdx) => likeBase ? base.GetFrameOfSubClip(frameIdx) : frameCount <= 0 || frameIdx<=0? 0 : (frameIdx-startFrame) % frameCount;
    static bool IsRequiredNow       (int a, int b, int c) => a < b && b < c;         // required now -> too late...  (same as IsInRange)
    static bool IsRequiredAheadFrms (int a, int b, int c, int d) => a < b+d && b < c;       // required ahead some frames
    static bool IsRequiredAheadCnks (int a, int b, int c, int d, int e) => (1+e)*a < b+e*c+d && b < c;   // require ahead by chunks and frames
    public bool IsRequired(int lastFrameReq) => lastFrameReq!=-1 && IsRequiredAheadCnks(startFrame, lastFrameReq, endFramePlusOne, buffAheadFrames, buffAheadChunks);
    public void DownloadFileSync(WebClient webClient)
    {
        webClient.DownloadFile(new Uri(URL), filePath);
#if EMULATE_NETWORK
        int timeEstimate = (endFramePlusOne-startFrame)*1000/30;
        System.Threading.Thread.Sleep(timeEstimate*8/10); // wait 80% of playing time
#endif
        downloaded = true;
    }
    public bool CloseIfPossible()
    {
        if (DateTime.UtcNow.Ticks - initTime < 5000)
            return false;
        Close();
        return true;
    }
}
// ------------------------------------------------------------------------------------------
// Same interface as a single file but streaming capable (composite pattern)
public class TetaviStreamComposite : TetaviStreamIfc
{
    bool rendering = false;
    protected Dictionary<int, TetaviStreamFile> comps = new Dictionary<int, TetaviStreamFile>();
    int frameCount, fps;
    int width, height;
    protected bool hasAudio = false;
    TetaviLoader loader;
    int lastFrameReq = -1;
    string mcInfo;
    TetaviStreamFile lastStreamUsed = null;   // Needed to activate the loader. Instead, it is possible to use sleepStrategy:false in TetaviLoaderFunc
    bool buffering = false;
    public TetaviStreamComposite(bool rendering = false) 
    {
        this.rendering = rendering;
    }

    protected virtual TetaviStreamFile CreateStreamFile(bool rendering) => new TetaviStreamFile(rendering);

    public void Open(string urlOrFilepath)
    {
        if (urlOrFilepath.ToLower().EndsWith(".tet"))
        {
            TetaviStreamFile stream = CreateStreamFile(rendering);
            comps.Add(0, stream);
            stream.Open(urlOrFilepath);
            frameCount = stream.GetFrameCount();
            width = stream.GetWidth();
            height = stream.GetHeight();
            mcInfo = stream.GetMaterialClassInfo();
            fps = stream.GetFps();
            lastStreamUsed = stream;
        }
        else
        {
            loader = new TetaviLoader(this, urlOrFilepath);
            List<int> fromFrame;
            List<string> name;
            List<string> url;
            if (loader.ReadManifest(out frameCount, out fps, out width, out height, out hasAudio, out fromFrame, out name, out url))
                for (int i = 0; i < name.Count; i++)
                {
                    int endFramePlusOne = i < name.Count - 1 ? fromFrame[i + 1] : frameCount;
                    TetaviStreamChunk stream = CreateStreamChunk(rendering, name[i], loader.Folder, loader.UrlBase, fromFrame[i], endFramePlusOne);
                    if (url.Count>i)
                        stream.URL = url[i];
                    comps.Add(fromFrame[i], stream);
                }
        }
    }

    public virtual TetaviStreamChunk CreateStreamChunk(bool r, string n, string f, string u, int f1, int f2) => new TetaviStreamChunk(r,n,f,u,f1,f2);

    public virtual void SetCalcNormals(bool calc)
    {
        if (comps.Count == 1)
            comps.First().Value.SetCalcNormals(calc);
        else
            foreach (var c in comps)
            {
                c.Value.SetCalcNormals(calc);
            }
    }

    public virtual void SetCalcTangents(bool calc)
    {
        if (comps.Count == 1)
            comps.First().Value.SetCalcTangents(calc);
        else
            foreach (var c in comps)
            {
                c.Value.SetCalcTangents(calc);
            }
    }

    public bool IsOpen()
    {
        return comps.Count > 0;
    }
    public void Close ()
    {
        if (loader!=null)
            loader.Close();
        foreach (var c in comps) c.Value.Close();
    }
    public virtual bool CanOpen() => true;
    public virtual bool IsInRange(int frameIdx) => true;
    public virtual int GetFrameOfSubClip(int frameIdx)
    {
        TetaviStreamFile subClip = GetSubClip(frameIdx);
        return subClip==null? 0 : subClip.GetFrameOfSubClip(frameIdx);
    }

    public TetaviStreamFile GetSubClip(int frameIdx)
    {
        if (comps.Count == 1)
            return comps.First().Value;
        frameIdx = frameIdx % frameCount;
        foreach (var c in comps)
        {
            if (c.Value.IsInRange(frameIdx))
                return c.Value;
        }
        return null;
    }
    public int GetFrameCount()
    {
        if (frameCount==-1 && comps.Count==1 && comps.First().Value.IsOpen())
            frameCount = comps.First().Value.GetFrameCount(); // non streaming (.tet file)
        return frameCount;
    }
    public virtual IntPtr GetFrameObj(int frameIdx, out int sid)
    {
        if (comps.Count > 1)  // so manifest read so frameCount!=-1
            frameIdx = frameIdx % frameCount;
        frameIdx = Math.Max(0, frameIdx);
        lastFrameReq = frameIdx;
        lock (this)
        {
            if (comps.Count()>0 && (!buffering || loader==null || !loader.Active))
            {
                var streamKeyVal = comps.Where(x => x.Value.IsInRange(frameIdx)).First();
                var stream = streamKeyVal.Value;
                buffering = !stream.IsOpen();
                if (stream!=lastStreamUsed)  // Yigal 10/6/21: Aleksey commented these 2 lines "it will not work" - ?
                    loader.Active = true;    // but changed back as they are required for streaming.
                lastStreamUsed = stream;
                if (!buffering)
                    return streamKeyVal.Value.GetFrameObj(frameIdx - streamKeyVal.Key, out sid);
                if (loader!=null)
                    loader.Active = true;
            }
            sid = -1;
            return IntPtr.Zero;
        }
    }
    public TetaviStreamChunk GetHighestPriority(bool ToLoadOrToOpen=true)
    {
        lock (this)
        {
            var reqList = comps.Where(x => (
                (TetaviStreamChunk)(x.Value)).IsRequired(lastFrameReq) && !x.Value.IsOpen() && ToLoadOrToOpen ^ x.Value.CanOpen());
            if (reqList.Count() == 0)
                return null;
            return (TetaviStreamChunk)(reqList.OrderBy(x => x.Key).First().Value);
        }
    }
    public void CloseOldStreams ()
    {
        foreach (var streamKeyVal in comps)
        {
            var stream = (TetaviStreamChunk)(streamKeyVal.Value);
            if (stream.IsOpen() && !stream.IsRequired(lastFrameReq))
                stream.CloseIfPossible();
        }
    }
    public int IsBuffering()
    {
        if (comps.Count == 1 && comps.First().Value.IsOpen())
            return comps.First().Value.IsBuffering(); // non streaming (.tet file)
        return buffering?1:0;
    }
    public int GetFramesDecodedAhead()
    {
        int acc=0; // todo: take only that that is not full (?)
        foreach (var c in comps)
            acc += c.Value.GetFramesDecodedAhead();
        return acc;
    }
    public string GetProfilingInfo()
    {
        foreach (var c in comps)
        {
            string s = c.Value.GetProfilingInfo();
            if (s != "")
                return s;
        }
        return "";
    }
    public string GetMatClassInfo()
    {
        if (comps.Count == 1 && comps.First().Value.IsOpen())
            return comps.First().Value.GetMatClassInfo(); // non streaming (.tet file)
        return ""; // TODO: in streaming, the manifest has to include a url of the material class info file (or no url if no mat class) 
    }
    public void SetTexturesN12Targets(IntPtr texY, IntPtr texUV, int w, int h)
    {
        foreach (var c in comps) // todo: this is switched every frame so: 1) keep the 2 switched set in the C++ side (Stream class members)
            if (c.Value.IsOpen())                                      //  2) Or: keep "active" streams and avoid looping all of them
                c.Value.SetTexturesN12Targets(texY, texUV, w, h);
    }
    public void SetTexturesYUV420Targets(IntPtr texY, IntPtr texU, IntPtr texV, int w, int h)
    {
        foreach (var c in comps)
            if (c.Value.IsOpen())
                c.Value.SetTexturesYUV420Targets(texY, texU, texV, w, h);
    }
    public virtual int GetFps() => fps;
    public virtual int GetWidth() => width;
    public virtual int GetHeight() => height;
    public virtual bool HasAudio() => false;
    public virtual bool AudioSync(int framePlaying, bool pause, ref bool audioPosDirty, ref int audioFrameRel) => false;
}