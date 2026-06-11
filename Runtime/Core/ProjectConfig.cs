using UnityEngine;
using RKode.Startup;

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
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded() {
        var config = Resources.Load<ProjectConfig>(StartupConstants.CONFIG_PATH);
        if (config == null) 
            return;

        string startUpSceneName = config.startupScene != null ? config.startupScene.name : STARTUP_SCENE;
        string loadingSceneName = config.loadingScene  != null ? config.loadingScene.name  : LOADING_SCENE;
        bool changed = config.startupSceneName != startUpSceneName || config.loadingSceneName != loadingSceneName;
        config.startupSceneName = startUpSceneName;
        config.loadingSceneName  = loadingSceneName;

        if (changed) { 
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssetIfDirty(config); 
        }
    }
#endif
}
}