using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{
    public ScenesPreset neverUnload;

    public float progress { get; private set; } = 0;
    public bool isLoading { get; private set; } = false;

    private struct SceneData
    {
        public int buildIndex;
        public bool isLoaded;
        public bool toLoad;
        public bool toUnload;

        public SceneData(int buildIndex, bool isLoaded, bool toLoad, bool toUnload)
        {
            this.buildIndex = buildIndex;
            this.isLoaded = isLoaded;
            this.toLoad = toLoad;
            this.toUnload = toUnload;
        }
    }

    private class SceneLoader
    {

        int activeScene;
        List<SceneData> scenes;

        public SceneLoader()
        {
            scenes = new List<SceneData>();
            activeScene = SceneManager.GetActiveScene().buildIndex;
        }

        public void AddLoadedScenes()
        {
            for (int i = 0; i < SceneManager.loadedSceneCount; i++)
            {
                int idx = SceneManager.GetSceneAt(i).buildIndex;
                scenes.Add(new SceneData(idx, true, false, true));

            }
        }

        public void AddScenesFromPreset(ScenesPreset preset)
        {
            List<int> toLoad = new List<int>();
            foreach (SceneSetup scene in preset.scenes)
            {
                int idx = SceneUtility.GetBuildIndexByScenePath(scene.path);
                SceneData data = new SceneData(-1, false, false, false);
                foreach (var item in scenes)
                {
                    if (idx == item.buildIndex)
                    {
                        data = item;
                        break;
                    }
                }

                if (data.buildIndex == -1)
                {
                    data = new SceneData(idx, false, true, false);
                    scenes.Add(data);
                }
                data.toLoad = true;
                if (scene.isActive) activeScene = idx;
            }
        }

        public void DontUnloadFromPreset(ScenesPreset preset)
        {
            foreach (SceneSetup scene in preset.scenes)
            {
                int idx = SceneUtility.GetBuildIndexByScenePath(scene.path);
                SceneData data = new SceneData(-1, false, false, false);
                foreach (var item in scenes)
                {
                    if (idx == item.buildIndex)
                    {
                        data = item;
                        break;
                    }
                }
                data.toUnload = false;
            }
        }

        public List<SceneData> getScenes()
        {
            return scenes;
        }
    }

    public void LoadFromPreset(ScenesPreset preset)
    {
        SceneLoader sceneLoader = new SceneLoader();

        sceneLoader.AddLoadedScenes();
        sceneLoader.AddScenesFromPreset(preset);
        sceneLoader.DontUnloadFromPreset(neverUnload);

        StartCoroutine(LoadScenes(sceneLoader));
    }

    IEnumerator LoadScenes(SceneLoader sceneLoader)
    {
        isLoading = true;
        progress = 0;

        List<SceneData> scenes = sceneLoader.getScenes();
        List<AsyncOperation> tasks = new List<AsyncOperation>();

        foreach (SceneData scene in scenes)
        {
            if (scene.isLoaded && scene.toUnload)
            {
                tasks.Add(SceneManager.LoadSceneAsync(scene.buildIndex, LoadSceneMode.Additive));
            }
            else if (!scene.isLoaded && scene.toLoad)
            {
                tasks.Add(SceneManager.UnloadSceneAsync(scene.buildIndex));
            }
        }

        while (true)
        {
            bool allDone = true;
            float localProgress = 0f;
            foreach (AsyncOperation task in tasks)
            {
                if (!task.isDone)
                {
                    allDone = false;
                }
                localProgress += task.progress;
            }

            if (allDone)
            {
                break;
            }

            progress = localProgress / tasks.Count;

            yield return null;
        }

        isLoading = false;
    }
}


