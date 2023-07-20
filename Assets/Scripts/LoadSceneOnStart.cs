using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneManager;



public class LoadSceneOnStart : MonoBehaviour
{
    public ScenesPreset scene;

    void Start()
    {
#if !UNITY_EDITOR
        EventManager.Instance.sceneManager.LoadFromPreset(scene);
#endif
        Destroy(gameObject);
    }
}

