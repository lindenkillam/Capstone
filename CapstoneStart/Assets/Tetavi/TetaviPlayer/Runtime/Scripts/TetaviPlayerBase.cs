using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.IO;
using UnityEngine;
using UnityEditor;
using static TetaviCAPI;
using UnityEngine.Networking;
using System.Threading;
using System.Net;
using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;


#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.Android;
#endif
[ExecuteInEditMode]
public class TetaviPlayerBase : MonoBehaviour
{
    public int FramePlaying  // use to position the progress bar controller 
    {
#if UNITY_EDITOR
        get => !Application.isPlaying? previewImpl.previewFrame
            :  (int)(Fps * ((DateTime.Now - startTime).TotalMilliseconds - AccumlatedPauseMs()) * speedCoef / 1000) + startFrameIdx;
#else
        get => (int)(Fps * ((DateTime.Now - startTime).TotalMilliseconds - AccumlatedPauseMs()) * speedCoef / 1000) + startFrameIdx;
#endif
        set
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                previewImpl.PreviewFrame(value);
            else
#endif
            SetFramePlaying(value);
            audioPosDirty = true;
        }
    }
    public int FrameCount { get; protected set; } = 0; // use to scale the progress bar controller 
    public bool Pause // use for a pause button
    {
        set
        {
            if (pauseState == value)
                return;
            if (!pauseState && value)
            {
                pausedTime = lastUpdate = DateTime.Now;
            }
            else //if (pauseState && !value) -- always
            {
                accumlatedPauseMs += (DateTime.Now - pausedTime).TotalMilliseconds;
                pausedTime = DateTime.Now;
                fpsDecodedLast = DateTime.Now;
            }
            pauseState = value;
        }
        get => pauseState;
    }
    
    public int Fps { get => fps == -1 ? 30 : fps; } // FPS of the clip. use to scale the progress bar from frames to seconds

    public String clipFile;
    public int has_material_segmentation  = 0; // 0-unlit  1-lit
    public bool isLit = false;
    public bool isTetaviSunc = false;

    public int timingMode = 0; // 1-by update   2-by update slow only
    public float speedCoef = 1.0f;
    public bool loopMode = true;
    public bool playOnAwakeMode = true;
    public float audioVolume = 1.0f;
    public bool debugMode = false;

    protected int FpsDecoded { get; set; } = 0;    // actual FPS consumed
    protected bool ActualTimeEvenIfDroppingFrames { get; set; } = false; // set to have the clip catch up, according to actual clock, when frame dropping
    protected bool CheckUserKeysMode { get; set; } = true; // set to false if controlling from application rather than testing/debugging keys

    protected TetaviStreamComposite stream;
    protected int startFrameIdx;
    protected DateTime startTime;
    protected DateTime pausedTime;
    protected DateTime lastUpdate;
    protected bool pauseState = false;
    public bool userPaused = false;
    protected bool[] userKeys = new bool[4];
    protected bool m_buffereing = false;
    protected double accumlatedPauseMs = 0;
    protected double fpsDiff;
    protected int currMeshFrameIndex = -1;
    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;
    protected Mesh[] saveForDestruction = new Mesh[3];
    protected int fps = -1; // known only after the first frame (should have been on the tet header as video has always constant fps)
    protected double fpsDecodedDelta;
    protected DateTime fpsDecodedLast;
    protected bool audioPosDirty = false;
    private AudioSource audioPlayer;
    protected Texture2D[] texY = new Texture2D[2];
    protected Texture2D[] texUV = new Texture2D[2];
    protected Texture2D[] texNM = new Texture2D[2];
    protected int[] tris;
    protected Vector4[] tangentsVec;
    protected int gop_id;
    protected IntPtr normal_map_ptr;
    protected bool streamStarted = false;

#if UNITY_EDITOR
    TetaviPreviewImpl previewImpl;
    public TetaviPlayerBase()
    {
        previewImpl = new TetaviPreviewImpl(this);
    }
    public void PreviewRefresh() => previewImpl.PreviewRefresh();
    public void PreviewStreamClose() { stream.Close(); }
    public void PreviewStartStream(string pathToFile) { StartStream(pathToFile, noAudio: true); }
    public void PreviewForceRefresh() { currMeshFrameIndex = -1; } // so next UpdateFrame will not return without doing anything
    public string PreviewFile()
    {
        gop_id = -1;
        string pathToFile;
        int type = StreamType(ref clipFile, out pathToFile);
        if (type < 1 || type > 2)  // url (type==3) sot supported to be shown in editor mode
            return "";
        return pathToFile;
    }
    public bool PreviewUpdateFrame(int previewFrame) => UpdateFrame(previewFrame);

    public int selFrame; // Serialized and set to PreviewMode by the Editor 
    public int frameCount = 1;
    public void PreviewMode(int frame)
    {
        if (Application.isPlaying)
            return;
        //player = GetComponent<TetaviPlayerBase>(); // GetComponent<TetaviEditorPlayerObj>(); -- did not work as a derived class that implements only SingleFromClip
        FramePlaying = frame;
        if (FrameCount>0)
            frameCount = FrameCount;
    }
#endif

    public void Awake ()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
            meshRenderer.enabled = false;
    }

    protected void OnDestroy()
    {
        if (stream != null)
        {
            stream.Close();
        }
    }

    protected virtual void CreateStreamDecoder()
    {
        stream = new TetaviStreamComposite();
    }

    IEnumerator CopyFile(string from, string to)
    {
        using (var uwr = UnityWebRequest.Get(from))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isHttpError || uwr.isNetworkError)
            {
                Debug.LogError($"Unity Download error :: {uwr.responseCode} - {uwr.error}!", this);
                yield break;
            }

            byte[] yourBytes = uwr.downloadHandler.data;
            Debug.LogError($"Unity lenght of copied bytes :: {yourBytes.Length}");

            File.WriteAllBytes(to, yourBytes);
        }
    }

    protected Mesh CreateMesh(IntPtr frame)
    {
        Mesh mesh = new Mesh();
        int faceCount = frame_face_count(frame);
        

        // Vertices
        int vertexCount = frame_get_vertex_count(frame);
        Vector3[] vertices = new Vector3[vertexCount];
        
        
        
        Vector2[] uv = new Vector2[vertexCount];
        GCHandle handle1 = GCHandle.Alloc(vertices, GCHandleType.Pinned);
        IntPtr pointer1 = handle1.AddrOfPinnedObject();
        frame_copy_vertex_xyz(frame, pointer1);
        //Faces
         // --- if it is not a key frame we don't need to use it
        //UV
        GCHandle handle3 = GCHandle.Alloc(uv, GCHandleType.Pinned);
        IntPtr pointer3 = handle3.AddrOfPinnedObject();
        frame_copy_uv_data(frame, true, pointer3);  // ---- if it is not a key frame we don't need to use it

        handle1.Free();
        
        handle3.Free();

        mesh.vertices = vertices;
        
        has_material_segmentation = stream.GetMatClassInfo().Length > 0 ? 1 : 0;
        
        //mesh.RecalculateNormals(); --  missly computed due to incontinuity of vertices after duplicating xyz to have different uv atlassing
        Vector3[] normals = new Vector3[vertexCount];
        GCHandle handle4 = GCHandle.Alloc(normals, GCHandleType.Pinned);
        IntPtr pointer4 = handle4.AddrOfPinnedObject();
        if (frame_copy_vertex_normals(frame, pointer4))
            mesh.normals = normals;
        // else warning "not set calculate normals ahead so they were not computed" (should not happen. test in streaming)
        handle4.Free();
        
        mesh.uv = uv;
        int gop_num = frame_gop_id(frame);
        //Debug.LogError($"Unity gop_id {gop_id} gop_num {gop_num}");
        // we are in a new gop
        //Debug.LogError($"gop {gop_id} faceCount*3 {faceCount * 3} vertexCount {vertexCount}");
        if (gop_id != gop_num || tris.Length != (faceCount*3))
        {
            
            tris = new int[faceCount * 3];
            GCHandle handle2 = GCHandle.Alloc(tris, GCHandleType.Pinned);
            IntPtr pointer2 = handle2.AddrOfPinnedObject();
            frame_copy_face_indices(frame, pointer2);
            handle2.Free();

        }
        mesh.triangles = tris;
#if UNITY_STANDALONE || UNITY_EDITOR
        if (CreateNormalMapTexture(frame))
        {
            float[] tangents = new float[vertexCount * 3];

            tangentsVec = new Vector4[vertexCount];

            GCHandle handle5 = GCHandle.Alloc(tangents, GCHandleType.Pinned);
            IntPtr pointer5 = handle5.AddrOfPinnedObject();
            if (frame_copy_tangents(frame, pointer5))
            {
                int j = 0;
                for (int i = 0; i < tangents.Length; i += 3)
                {
                    if (tangents[i] == 0 && tangents[i + 1] == 0 && tangents[i + 2] == 0)
                    {
                        tangents[i + 2] = 1;

                    }
                    tangentsVec[j] = new Vector4(tangents[i], tangents[i + 1], tangents[i + 2], 1);

                    j++;
                }

            }
            handle5.Free();
            mesh.tangents = tangentsVec;
            GetComponent<Renderer>().sharedMaterial.SetFloat("_NM", 1);
        }
        else
        {
            GetComponent<Renderer>().sharedMaterial.SetFloat("_NM", 0);
        }
#else
        //not mac or windows
        GetComponent<Renderer>().sharedMaterial.SetFloat("_Optimized", 1);

#endif



        GetComponent<Renderer>().sharedMaterial.SetFloat("_MS", has_material_segmentation);

        gop_id = gop_num;
        return mesh;
    }

    protected bool CreateNormalMapTexture(IntPtr frame)
    {
        int size = -1;
        int NM_width = -1;
        int NM_height = -1;


        IntPtr pixels = frame_get_normal_map(frame,ref size, ref NM_width, ref NM_height);
        
        
        if (pixels == null || size <= 0 || NM_width * NM_height != size)
        {
            if (!texNM[0]) return false;

            Resources.UnloadUnusedAssets();

            texNM[0].filterMode = FilterMode.Point;
            texNM[0].Apply();
            texNM[0] = null;
            return false;
        }

        texNM[0] = new Texture2D(NM_width, NM_height, TextureFormat.BC5, false);

        texNM[0].LoadRawTextureData(pixels, NM_width * NM_height);
        
        texNM[0].filterMode = FilterMode.Point;
        texNM[0].Apply();
        return true;
    }

    private Texture2D texture;
    protected void CreateTexture(IntPtr frame)
    {
        IntPtr textureObj = frame_get_texture_obj(frame);
        TextureInfo textureInfo = texture_get_info(textureObj);
        int Y_width = stream.GetWidth();
        int UV_width = stream.GetWidth() / 2;
        int Y_height = stream.GetHeight();
        int UV_height = stream.GetHeight() / 2;
        IntPtr pixels = textureInfo.pixels;
        if (texY[0] == null || texY[0].width != Y_width || texY[0].height != Y_height)
        {
            texY[0] = new Texture2D(Y_width, Y_height, TextureFormat.R8, false);
            texUV[0] = new Texture2D(UV_width, UV_height, TextureFormat.RG16, false);
        }
        texY[0].LoadRawTextureData(pixels, Y_width * 1 * Y_height);
        texUV[0].LoadRawTextureData(IntPtr.Add(pixels, Y_width * 1 * Y_height), UV_width * 2 * UV_height);
        texY[0].filterMode = FilterMode.Point;
        texUV[0].filterMode = FilterMode.Point;
        texY[0].Apply();
        texUV[0].Apply();
        

        meshRenderer.enabled = true;
    }
    protected int debugNoLoadMesh = 0; // use 1 to debug performance when mesh is created only once
    protected int debugNoLoadTexture = 0; // use 1 to debug performance when texture is created only once
    bool inStress = false;
    public bool UpdateFrame(int frameIdx)
    {
        if (frameIdx == currMeshFrameIndex)
            return true;
        int sid;
        IntPtr frame = stream.GetFrameObj(frameIdx, out sid);
        
        if (!UpdateFrame(frame, sid))
            return false;
        currMeshFrameIndex = frameIdx;
        return true;
    }
    virtual protected bool UpdateFrame(IntPtr frame, int sid)
    {
        
        if (!UpdateFramePrefix(frame))
            return false;

        if (debugNoLoadMesh >= 0)
        {
            meshFilter.mesh = CreateMesh(frame);
        }
            
        if (debugNoLoadMesh == 1)
            debugNoLoadMesh = -1;
        
        if (debugNoLoadTexture >= 0)
        {
            CreateTexture(frame);
            meshRenderer.sharedMaterial.SetTexture("_TexY", texY[0]);
            meshRenderer.sharedMaterial.SetTexture("_TexUV", texUV[0]);
            meshRenderer.sharedMaterial.SetTexture("_TexNM", texNM[0]);
        }
            
        if (debugNoLoadTexture == 1)
            debugNoLoadTexture = -1;
        UpdateFramePostfix();

        return true;
    }
    protected bool UpdateFramePrefix(IntPtr frame)
    {
        
        if (frame == IntPtr.Zero)
            return false;
        if (fps == -1)
            fps = stream.GetFps(); // assuming same fps along video but can not getting until reading the first frame
        return true;
    }

    protected void UpdateFramePostfix()
    {
        //frame_release(stream, frameIdx);  -- obsolete: API assumes it can free any returned frame when a new one is asked
        Resources.UnloadUnusedAssets();
        GC.Collect();
        fpsDecodedDelta = (DateTime.Now - fpsDecodedLast).TotalMilliseconds * 0.05 + fpsDecodedDelta * 0.95;
        fpsDecodedLast = DateTime.Now;
        FpsDecoded = (int)((1000f / (float)fpsDecodedDelta) + 0.5f);
    }

    protected double AccumlatedPauseMs()
    {
        return accumlatedPauseMs + (pauseState ? (DateTime.Now - pausedTime).TotalMilliseconds : 0);
    }

    protected void SetFramePlaying(int value)
    {
        startFrameIdx = value;
        accumlatedPauseMs = 0;
        startTime = pausedTime = lastUpdate = System.DateTime.Now;
        fpsDecodedLast = DateTime.Now;
    }

    public static int StreamType(ref string clipFile, out string pathToFile)
    {
        
        clipFile = clipFile.Replace(@"\", "/");
#if UNITY_ANDROID && !UNITY_EDITOR
        string assetsPrefix = Application.persistentDataPath.ToLower() + "/";
#else
#if UNITY_IOS
        string assetsPrefix = Application.streamingAssetsPath/*.ToLower()*/ + "/";
#else
        string assetsPrefix = Application.streamingAssetsPath.ToLower() + "/";
#endif
#endif
        if (clipFile!="" && clipFile != "..." && !clipFile.Contains("/") && !clipFile.Contains("url to download"))
        {
            clipFile = assetsPrefix + clipFile;
        }    
        if (clipFile != "" && clipFile != "..." && File.Exists(clipFile) && clipFile.ToLower().EndsWith(".tet"))
        {
            pathToFile = clipFile;
            if (clipFile.ToLower().StartsWith(assetsPrefix.ToLower()))
            {
                clipFile = clipFile.Substring(assetsPrefix.Length);
                return 1;
            }
            return 2;
        }
        if (clipFile.Contains("www.dropbox.com") || clipFile.Contains("url to download"))
        {
            pathToFile = clipFile;
            return 4;
        }
        if (clipFile.StartsWith("https://") || clipFile.StartsWith("http://") || clipFile.StartsWith("file://"))
        {
            pathToFile = clipFile;
            return 3;
        }
        pathToFile="";
        clipFile="...";
        return 0;
    }

    public bool hasAudio()
    {
        return stream.HasAudio();
    }

    protected virtual void StartStream(string pathToFile, bool noAudio=false)
    {
        
        register_debug_callback_default();
#if !UNITY_IOS && UNITY_EDITOR

        stream_set_progiling_debug_messages(false);
#endif
        CreateStreamDecoder();
        
        stream.Open(pathToFile);
        stream.SetCalcNormals(true);
        stream.SetCalcTangents(true);
        FrameCount = stream.GetFrameCount();
        if (stream.HasAudio() && !noAudio)
        {
            audioPlayer = gameObject.GetComponent<AudioSource>();
            audioPlayer.pitch = speedCoef;
            audioPlayer.volume = audioVolume;
            audioPlayer.loop = loopMode;
        }
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer==null)
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter==null)
            meshFilter = gameObject.AddComponent<MeshFilter>();
        SetFramePlaying(0); // do not use FramePlaying = 0; as it is recursive called from PreviewFrame
        if (!playOnAwakeMode)
            Pause = userPaused = true;
        streamStarted = true;
    }
    
    public bool StreamStarted()
    {
        return streamStarted;
    }

    protected IEnumerator Start()
    {
        if (clipFile == "") yield return null;
        gop_id = -1;
        if (!clipFile.Contains("\\") && !clipFile.Contains("/"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            yield return StartAndroid(clipFile);
#endif
        }
        string pathToFile;
        int type = StreamType(ref clipFile, out pathToFile);
        
        if (type>0)
        {
            if(type == 4)
            {
                StartCoroutine(DownloadFile(pathToFile));
                
                
            }
            else
            {

                StartStream(pathToFile);

            }
            
        }
        else
        {
            Debug.LogError($"Clip file by path {pathToFile} does not exist clip file is {clipFile}");
        }
        yield return null;
    }

    protected IEnumerator StartAndroid(string fileName)
    {
       
        string fromCopy = Path.Combine(Application.streamingAssetsPath, fileName);
        string toCopy = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(toCopy))
        {
            Debug.LogError($"Unity path from copy :: {fromCopy}, to copy :: {toCopy}");
            yield return CopyFile(fromCopy, toCopy);
        }
    }

    protected bool CheckUserKey(int keyIdx, string keyName)
    {
        if (Input.GetKeyDown(keyName) && !userKeys[keyIdx])
        {
            userKeys[keyIdx] = true;
            return true;
        }
        if (!Input.GetKeyDown(keyName))
            userKeys[keyIdx] = false;
        return false;
    }

    protected void Update()
    {
        if (isTetaviSunc)
            return;
        if (stream == null)
            return;
        fpsDiff = !Pause && timingMode!=0? (DateTime.Now-lastUpdate).TotalMilliseconds - 1000.0/Fps : 0; // positive if refresh rate takes longer (slower) than video's original ms between frames
        lastUpdate = DateTime.Now;
        if (timingMode==1 || fpsDiff>0)
        {
            accumlatedPauseMs += fpsDiff;
            if (fpsDiff!=0)
                audioPosDirty = true;
        }
        if (CheckUserKeysMode)
        {
            if (CheckUserKey(0, "space"))
            {
                userPaused = !userPaused;
                Pause = userPaused;
            }
            if (CheckUserKey(1, "="))
            {
                FramePlaying = FramePlaying + 100;
            }
            if (CheckUserKey(2, "-"))
            {
                FramePlaying = Math.Max(FramePlaying - 100, 0);
            }
            if (CheckUserKey(3, "x"))
            {
                ActualTimeEvenIfDroppingFrames = !ActualTimeEvenIfDroppingFrames;
            }
        }
        
        if (userPaused || !loopMode && FramePlaying >= FrameCount)
        {
            Pause = true;
            UpdateFrame(loopMode? FramePlaying : Math.Min(FramePlaying, FrameCount-1));
        }
        else
        {
            Pause = m_buffereing = !UpdateFrame(FramePlaying) && (!ActualTimeEvenIfDroppingFrames || 0 != stream.IsBuffering());

            if (m_buffereing)
            {
                inStress = false;
            }
            else
            {
                int a = stream.GetFramesDecodedAhead();

                if (inStress)
                {
                    inStress = a > 45;
                }
                else
                {
                    inStress = a < 15;
                }

                if (inStress)
                {
                    Pause = DateTime.Now.Millisecond % 2 == 0;  // slow down by half
                }
            }
        }
        if (audioPlayer)
        {
            audioPlayer.pitch = speedCoef;
            audioPlayer.volume = audioVolume;
            audioPlayer.loop = loopMode;
            int audioFrameRel=0;
            if (stream.AudioSync(FramePlaying, Pause, ref audioPosDirty, ref audioFrameRel))
                SetFramePlaying(audioFrameRel);
        }
    }


#if DEVELOPMENT_BUILD || UNITY_EDITOR // FPS display
    protected float elapsed = 0;

    protected string prof_txt = string.Empty;

    protected void OnGUI()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
        if (!debugMode)
            return;
        GUIStyle fpsProfillingGuiStyle = new GUIStyle(); //create a new variable
#if UNITY_ANDROID && !UNITY_EDITOR
        fpsProfillingGuiStyle.fontSize = 40;
#else
        fpsProfillingGuiStyle.fontSize = 20;
#endif
        int verticalPos = 10;
        if (stream == null)
        {
            GUI.Label(new Rect(10, verticalPos, 300, 100), "Clip file Missing", fpsProfillingGuiStyle);
            return;
        }

        elapsed += Time.deltaTime;
        if (elapsed >= 0.3f)
        {
            elapsed %= 0.3f;
            string text = FpsDecoded.ToString("D3") + " FPS Decoded" + (m_buffereing ? " Bufferring..." : "") + (inStress ? " Slowing" : "");
            if (fpsDiff!=0)
            {
                int a = (int)(fpsDiff+0.5);
                text = text + " refresh delay=" + a.ToString("D3") + "ms";
            }
            string prof_info = stream.GetProfilingInfo();
            if (!string.IsNullOrEmpty(prof_info))
            {
                text = text + "\n" + prof_info;
            }

            prof_txt = text;
        }

        if (!string.IsNullOrEmpty(prof_txt))
        {
            GUI.Label(new Rect(10, verticalPos, 10, 10), prof_txt, fpsProfillingGuiStyle);
        }
    }
#endif


    IEnumerator DownloadFile(string path)
    {
        if (path.EndsWith("0"))
        {
            // example https://www.dropbox.com/s/c45c568232cdxq2/boxole_2_6.0k.tet?dl=0
            // change the 0 to 1 at the end of path for direct download
            path = path.Remove(path.Length - 1, 1) + "1";
        }
        Debug.Log("DownloadFile " + path);
        var uwr = new UnityWebRequest(path, UnityWebRequest.kHttpVerbGET);
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("User-Agent", "DefaultBrowser");
        uwr.SetRequestHeader("Cookie", string.Format("DummyCookie"));
        uwr.chunkedTransfer = false;
        uwr.downloadHandler = new DownloadHandlerFile(Application.persistentDataPath + "/tet.tet");
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(uwr.error);
        }
        else
        {

            StartStream(Application.persistentDataPath + "/tet.tet");

            Debug.Log("File successfully downloaded and saved to " + Application.persistentDataPath + "/tet.tet");
        }
    }

    public void ChangeTo(string path)
    {
        stream.Close();
        clipFile = path;
        
        StartCoroutine(Start());
    }
}
