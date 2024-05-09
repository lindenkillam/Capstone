using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TetaviPlayer))]
public class TetaviPlayerEditor : Editor 
{
    SerializedObject serObj;
    bool reStarted;
    SerializedProperty clipFile;
    //SerializedProperty unlitMode;
    SerializedProperty timingMode;
    SerializedProperty speedCoef;
    SerializedProperty loopMode;
    SerializedProperty playOnAwakeMode;
    SerializedProperty audioVolume;
    SerializedProperty debugMode;
    SerializedProperty selFrame;
    SerializedProperty frameCount;

    void OnEnable()
    {
        serObj = new SerializedObject(target);
        reStarted = true;

        selFrame = serObj.FindProperty("selFrame");
        frameCount = serObj.FindProperty("frameCount");

        clipFile = serObj.FindProperty("clipFile");
        //unlitMode = serObj.FindProperty("unlitMode");
        speedCoef = serObj.FindProperty("speedCoef");
        timingMode = serObj.FindProperty("timingMode");
        loopMode = serObj.FindProperty("loopMode");
        playOnAwakeMode = serObj.FindProperty("playOnAwakeMode");
        audioVolume = serObj.FindProperty("audioVolume");
        debugMode = serObj.FindProperty("debugMode");
    }

    public override void OnInspectorGUI()
    {
        serObj.Update();
        bool refresh = false;

        TetaviPlayerBase obj = (TetaviPlayerBase)target;
        selFrame.intValue = (int)EditorGUILayout.Slider(new GUIContent("Preview Frame: "), selFrame.intValue, 0, frameCount.intValue);
        obj.PreviewMode(selFrame.intValue);
        frameCount.intValue = obj.frameCount;
        GUILayout.Space(10);

        // Clip 
        string pathToFile, clipFileV = clipFile.stringValue;
        int selectedSource = TetaviPlayerBase.StreamType(ref clipFileV, out pathToFile);
        string[] optionsSource = new string[] { "Select...", "Asset", "File", "URL","Download" };
        int selectedSourceNew = EditorGUILayout.Popup("Source", selectedSource, optionsSource);
        if (selectedSourceNew!= selectedSource)
        {
            selectedSource = selectedSourceNew;
            string defaultFolder = "C:\\"; // TODO: look in Downloads as https://stackoverflow.com/questions/10667012/getting-downloads-folder-in-c
            if (selectedSource == 3)
                clipFileV = selectedSource == 3 ? "http://" : selectedSource == 2 ? defaultFolder : "...";
            else if (selectedSource == 2)
                clipFileV = EditorUtility.OpenFilePanel("Select Clip File", defaultFolder, "tet");
            else if (selectedSource == 1)
                clipFileV = EditorUtility.OpenFilePanel("Select Clip Asset", Application.streamingAssetsPath, "tet");
            else if (selectedSource == 4)
                clipFileV = "url to download";
            else
                clipFileV = "...";
            refresh = true;
        }
        TetaviPlayerBase.StreamType(ref clipFileV, out pathToFile); // update clipFileV to remove streamingAssetsPath and replace \ by /
        clipFile.stringValue = clipFileV;
        EditorGUILayout.PropertyField(clipFile, new GUIContent("Clip"));
        GameObject go = Selection.activeGameObject;
        if (go.GetComponent<TetaviPlayer>().has_material_segmentation > 0 && go.GetComponent<Renderer>().sharedMaterial.shader.name.Contains("Unlit"))
            EditorGUILayout.HelpBox("You are using tet with material segmentations, assign TetaviLit Material to access them", MessageType.Warning);


        //Playback
        EditorGUILayout.LabelField("Playback", EditorStyles.boldLabel);

        string[] optionsSync = new string[] { "Real Time", "Unity Clock" };
        int selectedSync = timingMode.intValue;
        int newSync = EditorGUILayout.Popup("Sync Type", selectedSync, optionsSync);
        timingMode.intValue = newSync;

        /*bool slowDown = EditorGUILayout.Toggle("Slow for movie capture", timingMode.intValue == 2);
        int timingModeParam = slowDown ? 2 : 0;   // 0-"Actual", 1-"Refresh rate", 2-"Refresh rate - slow only"
        if (timingModeParam != timingMode.intValue)
        {
            timingMode.intValue = timingModeParam;
            refresh = true;
        }*/

        EditorGUI.BeginDisabledGroup(timingMode.intValue != 0);
        speedCoef.floatValue = EditorGUILayout.Slider("Speed", speedCoef.floatValue, 0.2F, 2F);
        EditorGUI.EndDisabledGroup();
        loopMode.boolValue = EditorGUILayout.Toggle("Loop", loopMode.boolValue);
        playOnAwakeMode.boolValue = EditorGUILayout.Toggle("Play On Awake", playOnAwakeMode.boolValue);
        EditorGUILayout.Space();

        //Audio
        EditorGUILayout.LabelField("Audio", EditorStyles.boldLabel);
        //string[] optionsAudio = new string[]
        //{
        //    "Audio Source", "Direct", "None", 
        //};
        //selectedAudio = EditorGUILayout.Popup("Type", selectedAudio, optionsAudio); 
        audioVolume.floatValue = EditorGUILayout.Slider("Volume", audioVolume.floatValue, 0, 1);
        EditorGUILayout.Space();

        //Debug
        EditorGUILayout.Space();
        debugMode.boolValue = EditorGUILayout.Toggle("Show Debug Info", debugMode.boolValue);

        serObj.ApplyModifiedProperties();

        if (!go.activeInHierarchy)
            reStarted = true;
        if (!EditorApplication.isPlaying && go.activeInHierarchy && (refresh || reStarted))
        {
            (target as TetaviPlayerBase).PreviewRefresh();
            reStarted = false;
        }
    }

}
