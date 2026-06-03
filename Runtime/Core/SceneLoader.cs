using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RKode.Utils {
public class SceneLoader : Singleton<SceneLoader> {
    private static ProjectConfig _config;
    private static ProjectConfig Config => 
        _config ??= Resources.Load<ProjectConfig>("RKode/ProjectConfig");

    public Action<float> onProgressUpdate;
    private Coroutine _loadingCO;

    public void LoadScene(string sceneName, Action onLoadCallback = null, bool showLoading = true, float initialDelay=.2f) { 
        if(_loadingCO != null) {
            Debug.Log("Another Scene is loading...");
            return;
        }

        _loadingCO = StartCoroutine(LoadSceneInternal(sceneName, onLoadCallback, showLoading, initialDelay));
    }

    public IEnumerator LoadSceneInternal(string sceneName, Action onLoadCallback = null, bool showLoading = true, float initialDelay=.2f) {
        if(showLoading) {
            LoadAdditiveScene(Config.loadingSceneName);
            yield return new WaitForSeconds(.1f);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        StartCoroutine(ExecuteCallback(() => {
            if(showLoading) {
                UnloadScene(Config.loadingSceneName);
            }
            onLoadCallback?.Invoke();
            _loadingCO = null;
        }, initialDelay, () => {
            var progress = Mathf.Clamp01(asyncLoad.progress / .9f);
            if (progress >= 0.9f) {
                asyncLoad.allowSceneActivation = true;
            }

            onProgressUpdate?.Invoke(progress);
            Debug.Log($"Progress: {progress}");

            return asyncLoad.isDone;
        }));
    }

    public IEnumerator ExecuteCallback(Action callback, float initialDelay = 1f, Func<bool> predicate = null) {
        yield return new WaitForSeconds(initialDelay);

        if(predicate != null) {
            yield return new WaitUntil(predicate);
        }
        callback?.Invoke();
    }

    public void LoadAdditiveScene(string sceneName) {
        if(SceneManager.GetSceneByName(sceneName).isLoaded){
            Debug.Log("Scene is already loaded...");
            return;
        }

        Debug.Log($"Loading {sceneName} additively...");
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadScene(string sceneName) {
        if(SceneManager.GetSceneByName(sceneName).isLoaded){
            Debug.Log($"Loading {sceneName} unloaded...");
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}
}
