using System;
using UnityEditor;
using UnityEngine;

public class TetaviMenu : MonoBehaviour
{
    // Add a menu item named "Do Something" to MyMenu in the menu bar.

    [MenuItem("GameObject/Tetavi/Tetavi lit Player", false, 10)]
    static void TetaviTetaviPlayerMenuLit(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("TetaviPlayer (lit)");
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        // Add TetaviPlayer script to the Gameobject
        go.AddComponent<TetaviPlayer>();
        go.AddComponent<AudioSource>();
        go.AddComponent<UnityEngine.MeshRenderer>();
        go.transform.rotation = new Quaternion(0, -(float)(Math.Sin(Math.PI/4)), -(float)(Math.Sin(Math.PI/4)), 0);
        go.transform.localScale = new Vector3(1, -1, 1);
        go.GetComponent<Renderer>().material =  new Material(Shader.Find("Tetavi/TetaviShaderLitTransparent"));
        go.GetComponent<TetaviPlayer>().isLit = true;
        go.GetComponent<TetaviPlayer>().clipFile = @"DancerLitWithMS.tet";
        Selection.activeObject = go;
    }

    [MenuItem("GameObject/Tetavi/Tetavi unlit Player")]
    static void TetaviPlayerMenuUnlit(MenuCommand tetaviMenu)
    {
        // Create a custom game object
        GameObject go = new GameObject("TetaviPlayer (unlit)");
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, tetaviMenu.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        // Add TetaviPlayer script to the Gameobject
        go.AddComponent<TetaviPlayer>();
        go.AddComponent<AudioSource>();
        go.AddComponent<UnityEngine.MeshRenderer>();
        go.transform.rotation = new Quaternion(0, -(float)(Math.Sin(Math.PI / 4)), -(float)(Math.Sin(Math.PI / 4)), 0);
        go.transform.localScale = new Vector3(1, -1, 1);
        go.GetComponent<Renderer>().material = new Material(Shader.Find("Tetavi/TetaviDefaultShaderUnlit"));
        go.GetComponent<TetaviPlayer>().clipFile = @"KickboxingChampion.tet";
        go.GetComponent<TetaviPlayer>().isLit = false;
        Selection.activeObject = go;
    }

    [MenuItem("GameObject/Tetavi/Tetavi Sync")]
    static void TetaviPlayerMenuSync(MenuCommand tetaviMenu)
    {
        // Create a custom game object
        GameObject go = new GameObject("Tetavi Sync");
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, tetaviMenu.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        // Add TetaviPlayer script to the Gameobject
        go.AddComponent<TetaviSync>();
    }

    /* Adding On the main menu a Tetavi menu
    [MenuItem("Tetavi/Tetavi Player")]
    static void TetaviMenu(MenuCommand tetaviMenu){
    ...
    }*/
}
