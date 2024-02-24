using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine;
using static TetaviCAPI;
using UnityEditor;
using System.Threading;
using System.Net;
using System.Linq;

// C# class that uses multithreading.
// => Do not use Unity API here

// Generic Thread class
public abstract class TetaviLoaderThread
{
    Thread thread;
    ManualResetEvent signalEvent = new ManualResetEvent(false);
    bool stop = false, active = false, sleepStrategy;
    int sleepTimeout;
    
    public TetaviLoaderThread (bool sleepStrategy=false, int sleepTimeout=500)
    {
        this.sleepStrategy = sleepStrategy;
        this.sleepTimeout = sleepTimeout;
        thread = new Thread(new ThreadStart(this.StartFunc));
        thread.Start();
    }
    ~TetaviLoaderThread()
    {
        if (thread != null)
            Close();
    }
    public void Close()
    {
        stop = true;
        signalEvent.Set();
        if (!thread.Join(5000))
        {
            UnityEngine.Debug.LogError("Could not stop loader thread");
        }
        thread = null;
    }
    public bool Active
    {
        get => active;
        set
        {
            if (active != value)
            {
                active = value;
                if (active)
                    signalEvent.Set();
                else
                    signalEvent.Reset();
            }
        }
    }
    protected abstract bool Main();
    void StartFunc()
    {
        while (!stop)
        {
            if (active || !sleepStrategy)
            {
                active = Main();
            }
            if (!active)
            {
               signalEvent.WaitOne(sleepTimeout);
            }
        }
    }
}
// ------------------------------------------------------------------------------------------
// Generic Thread class using lambda
public class TetaviLoaderFunc : TetaviLoaderThread
{
    Func<TetaviLoader, bool> func;
    TetaviLoader loader;
    public TetaviLoaderFunc(TetaviLoader loader, Func<TetaviLoader, bool> func, bool sleepStrategy=false, int sleepTimeout=500)
        : base(sleepStrategy, sleepTimeout)
    {
        this.func = func;
        this.loader = loader;
    }
    protected override bool Main() => func(loader);
}
// ------------------------------------------------------------------------------------------
// Main class for a loader that is part of streaming
public class TetaviLoader
{
    TetaviLoaderFunc func1, func2;
    string folder, urlBase, name, ext, drpb;
    TetaviStreamComposite streamRef;
    public string Folder { get => folder; }
    public string Name { get => name; }
    public string UrlBase { get => urlBase; }
    
    public TetaviLoader(TetaviStreamComposite streamRef, string url)
    {
        this.streamRef = streamRef;
        int i = url.LastIndexOfAny(new char [] {'\\','/'});
        if (i>0)
        {
            urlBase = url.Substring(0, i);
            name = url.Substring(i+1);
        }
        ext = ".manifest";
        drpb = "";
        if (name.EndsWith(".drpb"))  // download from drop box requires an accurate extension type
        {
            ext = ".drpb";
            drpb = "?dl=1";
            name = name.Substring(0, name.Length-5);
        }
        if (url.EndsWith("/") || urlBase=="" || name=="" || name.Contains("."))
        {
            UnityEngine.Debug.LogError("Unexpected URL format (actually looking for url.manifest file");
            return;
        }
       
        folder = Application.temporaryCachePath + "/" + Name;
        folder = new System.IO.FileInfo(folder).FullName;
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        func1 = new TetaviLoaderFunc(this, (o) => o.Main1(), sleepStrategy: true); // option: use sleepStrategy: false and sleepTimeout: 100
        func2 = new TetaviLoaderFunc(this, (o) => o.Main2(), sleepStrategy: true); //         instead of 'if (stream!=lastStreamUsed) loader.Active = true;' in TetaviStream
    }
    public bool ReadManifest (out int frameCount, out int fps, out int width, out int height, out bool hasAudio, out List<int> fromFrames, out List<string> names, out List<string> urls)
    { 
        string manifestPath = folder + "/" + name + ext;
         if (!File.Exists(manifestPath))
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(urlBase + "/" + name + ext + drpb, manifestPath);
        }
        TetaviLoaderManifest m = new TetaviLoaderManifest();
        bool ok = m.Read(manifestPath);
        frameCount = m.FrameCount;
        fps = m.Fps;
        width = m.Width;
        height = m.Height;
        hasAudio = m.HasAudio==1;

        fromFrames = m.FromFrames;
        names = m.Names;
        urls = m.URLs;
        for (int i=0; i<urls.Count; i++) urls[i]+=drpb;
        return ok;
    }
    List<Thread> parallel = new List<Thread>();
    protected bool Main1()
    {
        TetaviStreamChunk stream = streamRef.GetHighestPriority();
        if (stream == null)
            return false;
        stream.DownloadFileSync(new WebClient());
        func2.Active = true; 
        return true;
    }
    protected bool Main2()
    {
        TetaviStreamChunk stream = streamRef.GetHighestPriority(false);
        if (stream == null)
            return false;
        stream.Open(null);
        int dum;
        stream.GetFrameObj(0, out dum); // must ask for the first frame for decoding to be done
        streamRef.CloseOldStreams();
        return true;
    }

    public void Close() => func1.Close();
    public bool Active
    {
        get => func1.Active || func2.Active;
        set { func1.Active = value; func2.Active = value; }
     }
}
// ------------------------------------------------------------------------------------------
// Class to tread the streaming manifest. todo: use real json parser
class TetaviLoaderManifest
{
    public int FrameCount { get; private set; } = -1;
    public int Fps { get; private set; } = -1;
    public int Width { get; private set; } = -1;
    public int Height { get; private set; } = -1;
    public int HasAudio { get; private set; } = -1;
    public List<int> FromFrames { get; } = new List<int>();
    public List<string> Names { get; } = new List<string>();
    public List<string> URLs { get; } = new List<string>();
    // READ THE JSON FILE. Actually all JSON staff is ignored beside that:
    //            frameCount :
    //            fps :
    //            ...
    //            0,chunk1.tet
    //            100,chunk1.tet
    //            200,chunk1.tet
    //            ...
    public TetaviLoaderManifest()
    {
    }
    public bool Read(string manifestPath)
    {
        byte[] bytes = File.ReadAllBytes(manifestPath);
        string str = System.Text.Encoding.UTF8.GetString(bytes);
        string[] lines = str.Split('\n', '\r');
        for (int i = 0; i < lines.Length; i++)
        {
            string line0 = lines[i];
            string line = line0.Replace(" ", "").Replace("\t", "").Replace("\"", "").Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "");
            if (line.EndsWith(","))
                line = line.Substring(0, line.Length - 1);
            if (string.IsNullOrEmpty(line))
                continue;
            string[] kv = line.Split(':');
            string[] tc = line.Split(',');
            bool ok = false;
            if (tc.Length == 2 || tc.Length == 3)
            {
                int val;
                ok = int.TryParse(tc[0], out val);
                if (ok)
                {
                    FromFrames.Add(val);
                    Names.Add(tc[1]);
                    if (tc.Length == 3)
                        URLs.Add(tc[2]);
                }
            }
            else if (kv.Length == 2)
            {
                foreach (PropertyInfo property in this.GetType().GetProperties())
                {
                    if (kv[0].ToLower() == property.ToString().Replace("Int32 ","").ToLower())
                    {
                        int val=-1;
                        ok = int.TryParse(kv[1], out val);
                        property.SetValue(this, val);
                        break;
                    }
                }
            }
            if (!ok && line!="Chunks:")  // "Chunks:" is needed to have a JSON syntax, but actually simply ignored
            {
                UnityEngine.Debug.LogError("Could not parse line from " + manifestPath + " : " + line);
            }
        }
        foreach (PropertyInfo property in this.GetType().GetProperties())
        {
            if (property.GetType() == typeof(int) && (int)(property.GetValue(this, null)) == -1)
            {
                UnityEngine.Debug.LogError("Missing property in " + manifestPath + " : " + property.Name);
                return false;
            }
        }
        if (FromFrames.Count == 0)
        {
            UnityEngine.Debug.LogError("No tet chunks list in " + manifestPath);
            return false;
        }
        if (FromFrames[0]!=0)
        {
            UnityEngine.Debug.LogError("No chunk stat starts at 0 in " + manifestPath);
            return false;
        }
        return true;
    }
}
