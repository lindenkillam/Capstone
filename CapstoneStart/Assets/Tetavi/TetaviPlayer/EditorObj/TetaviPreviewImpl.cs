using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

public class TetaviPreviewImpl
{
    public int previewFrame = -1, previewFrameDone = -1;
    protected TetaviPlayerBase parent;

    public TetaviPreviewImpl(TetaviPlayerBase parent)
    {
        this.parent = parent;
    }
    public void PreviewRefresh()
    {
        if (previewFrame != -1)
            PreviewRefreshFrame(previewFrame);
    }
    public void PreviewFrame(int frame)
    {
        if (frame == previewFrame)
            PreviewUpdate();
        else
            PreviewRefreshFrame(frame);
    }
    protected void PreviewRefreshFrame(int frame)
    {
        if (previewFrame != -1 && previewFrameDone == -1)
        {
            parent.PreviewStreamClose();
            previewFrame = -1;
        }
        string pathToFile = parent.PreviewFile();
        if (pathToFile=="")  // url (type==3) sot supported to be shown in editor mode
            return; // TODO: show a box
        parent.PreviewStartStream(pathToFile); // StartStream(pathToFile, noAudio: true);
        parent.PreviewForceRefresh(); // currMeshFrameIndex = -1; // so next UpdateFrame will not return without doing anything
        previewFrame = frame;
        previewFrameDone = -1;
#if UNITY_EDITOR
        EditorApplication.delayCall += PreviewUpdate;
#endif
    }
    protected void PreviewUpdate()
    {
        if (previewFrame != -1 && previewFrameDone == -1 && parent.PreviewUpdateFrame(previewFrame))
        {
            parent.PreviewStreamClose();
            previewFrameDone = previewFrame;
        }
#if UNITY_EDITOR
        else if (previewFrameDone != previewFrame)
            EditorApplication.delayCall += PreviewUpdate;
#endif
    }
}

#endif // UNITY_EDITOR