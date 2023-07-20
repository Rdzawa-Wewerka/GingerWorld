using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    public GameObject panel;
    public Slider bar;

    private void OnEnable()
    {
        panel.SetActive(false);
        EventManager.Instance.sceneManager.LoadingStart += OnLoadingStart;
        EventManager.Instance.sceneManager.LoadingEnd += OnLoadingEnd;
        EventManager.Instance.sceneManager.LoadingProgress += OnLoadingProgress;
    }

    private void OnDestroy()
    {
        EventManager.Instance.sceneManager.LoadingStart -= OnLoadingStart;
        EventManager.Instance.sceneManager.LoadingEnd -= OnLoadingEnd;
        EventManager.Instance.sceneManager.LoadingProgress -= OnLoadingProgress;
    }

    private void OnLoadingStart()
    {
        panel.SetActive(true);
        bar.value = 0;
    }

    private void OnLoadingEnd()
    {
        panel.SetActive(false);
    }

    private void OnLoadingProgress(float progress)
    {
        bar.value = progress;
    }
}
