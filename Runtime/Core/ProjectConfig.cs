using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RKode {
public class ProjectConfig : ScriptableObject {
    public const string STARTUP_SCENE = "Bootstrap";
    public const string LOADING_SCENE = "Loading";

    public string startupSceneName = STARTUP_SCENE;
    public string loadingSceneName = LOADING_SCENE;

#if UNITY_EDITOR
    public SceneAsset startupScene;
    public SceneAsset loadingScene;

    private void OnValidate() {
        startupSceneName = startupScene != null ? startupScene.name : STARTUP_SCENE;
        loadingSceneName = loadingScene != null ? loadingScene.name : LOADING_SCENE;

        EditorUtility.SetDirty(this);
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded() {
        var config = Resources.Load<ProjectConfig>("RKode/ProjectConfig");
        if (config == null) 
            return;

        config.startupSceneName = config.startupScene != null ? config.startupScene.name : STARTUP_SCENE;
        config.loadingSceneName = config.loadingScene != null ? config.loadingScene.name : LOADING_SCENE;

        EditorUtility.SetDirty(config);
    }
#endif
}
}