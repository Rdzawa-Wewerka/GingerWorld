using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;


class SectorGenerator : EditorWindow
{
    [MenuItem("Window/SectorGenerator")]
    public static void ShowWindow()
    {
        SectorGenerator.GetWindow(typeof(SectorGenerator));
    }

    private const string ChunksFolder = "Chunks";
    private const string MaterialsFolder = "Materials/Terrains";
    private const string PrefabOutFolder = "Prefabs/Terrains";

    private int _selected = 0;
    string[] _options = null;

    private void OnGUI()
    {
        _options = GetSectorFolders();
        EditorGUI.BeginChangeCheck();

        this._selected = EditorGUILayout.Popup("My Simple Dropdown", _selected, _options);

        if (EditorGUI.EndChangeCheck())
        {
            Debug.Log(_options[_selected]);
        }

        if (GUILayout.Button("CreateSector"))
        {
            CreateSector();
        }
    }

    private void CreateSector()
    {
        string localPath = "Assets/" + PrefabOutFolder + "/" + _options[_selected] + ".prefab";
        GameObject parent = new GameObject();
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/" + ChunksFolder + "/" + _options[_selected]);
        FileInfo[] info = dir.GetFiles("*.obj");
        int minX = Int32.MaxValue;
        int minY = Int32.MaxValue;
        int maxX = Int32.MinValue;
        int maxY = Int32.MinValue;
        var allowMaterials = GetMaterials();
        foreach (FileInfo f in info)
        {
            GameObject chunkModel = (GameObject)AssetDatabase.LoadAssetAtPath(
                "Assets/" + ChunksFolder + "/" + _options[_selected] + "/" + f.Name, typeof(GameObject));
            GameObject chunk = Instantiate(chunkModel, Vector3.zero, Quaternion.identity);
            int x = Int32.Parse(chunk.name.Split('_')[0]);
            int y = Int32.Parse(chunk.name.Split('_')[1].Split("(")[0]);
            chunk.transform.position = new Vector3(-x, 0, y);
            if (minX > x) minX = x;
            if (minY > y) minY = y;
            if (maxX < x) maxX = x;
            if (maxY < y) maxY = y;
            var allChildren = chunk.transform.Cast<Transform>().ToArray();
            foreach (Transform child in allChildren)
            {
                if (!allowMaterials.ContainsKey(child.gameObject.name))
                {
                    DestroyImmediate(child.gameObject);
                    continue;
                }

                List<Material> materials = new List<Material>();
                for (int i = 0; i < child.GetComponent<MeshRenderer>().sharedMaterials.Length; i++)
                    materials.Add(allowMaterials[child.gameObject.name]);
                child.GetComponent<MeshRenderer>().sharedMaterials = materials.ToArray();
                child.AddComponent<MeshCollider>();
            }

            chunk.transform.parent = parent.transform;
        }

        parent.transform.position = new Vector3((minX + maxX) / 2.0f, 0, (minY + maxY) / 2.0f);
        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAssetAndConnect(parent, localPath, InteractionMode.UserAction, out prefabSuccess);
        if (prefabSuccess)
            Debug.Log("Sector was saved successfully");
        else
            Debug.Log("Sector failed to save" + prefabSuccess);

        DestroyImmediate(parent);
    }

    private Dictionary<string, Material> GetMaterials()
    {
        Dictionary<string, Material> result = new Dictionary<string, Material>();
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/" + MaterialsFolder);
        FileInfo[] info = dir.GetFiles("*.mat");
        foreach (FileInfo f in info)
        {
            Material material = (Material)AssetDatabase.LoadAssetAtPath(
                "Assets/" + MaterialsFolder + "/" + f.Name, typeof(Material));
            result.Add(Path.GetFileNameWithoutExtension(f.Name), material);
        }

        return result;
    }

    private string[] GetSectorFolders()
    {
        var folders = AssetDatabase.GetSubFolders("Assets/" + ChunksFolder);
        folders = new List<string>(folders).Select(x => x.Split("/").Last()).ToArray();
        return folders;
    }
}