using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
using System.Linq;

public class ScenesPreset : ScriptableObject
{
    public SceneSetup[] Scenes;

    public void EditorLoad()
    {
        EditorSceneManager.RestoreSceneManagerSetup(Scenes);
        Debug.Log(string.Format("Scene setup restored"));
    }
}
