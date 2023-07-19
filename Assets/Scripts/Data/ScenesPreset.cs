using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor.SceneManagement;

public class ScenesPreset : ScriptableObject
{
    public SceneSetup[] scenes;

    public void EditorLoad()
    {
        EditorSceneManager.RestoreSceneManagerSetup(scenes);
        Debug.Log(string.Format("Scene setup restored"));
    }
}
