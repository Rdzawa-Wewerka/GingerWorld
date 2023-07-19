using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UIElements;

class SceneWindow : EditorWindow
{
    [MenuItem("Window/SceneWindow")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SceneWindow));
    }

    private List<ScenesPreset> GetAllScenesPreset()
    {
        var guids = AssetDatabase.FindAssets("t:" + typeof(ScenesPreset).Name);
        List<ScenesPreset> assets = new List<ScenesPreset>();
        foreach (var guid in guids)
        {
            var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(ScenesPreset));
            assets.Add(obj as ScenesPreset);
        }
        return assets;
    }

    private void OnGUI()
    {
        List<ScenesPreset> presets = GetAllScenesPreset();
        GUILayout.BeginScrollView(Vector2.zero);
        foreach (var preset in presets)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(preset.name);
            if (GUILayout.Button("Load"))
            {
                preset.EditorLoad();
            }

            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }
}
