using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! THE SINGLETON 
// Do not make more singletons
// The one to rule them all, the one to find them, the one to bring them all and in the darkness bind them :-)

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    EventManager()
    {

    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public SceneManager.SceneManager sceneManager;

}
