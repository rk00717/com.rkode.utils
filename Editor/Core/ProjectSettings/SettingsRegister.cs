#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace RKode.Editor.ProjectSettings {
internal static class SettingsRegister {
    private const string CONFIG_PATH = "Assets/Resources/RKode/ProjectConfig.asset";
    private const string RESOURCES_PATH = "RKode/ProjectConfig";

    [InitializeOnLoadMethod]
    private static void Initialize() {
        EnsureConfigExists();
    }

    internal static ProjectConfig EnsureConfigExists() {
        var config = Resources.Load<ProjectConfig>(RESOURCES_PATH);
        if (config != null) 
            return config;

        config = ScriptableObject.CreateInstance<ProjectConfig>();

        System.IO.Directory.CreateDirectory("Assets/Resources/RKode");
        AssetDatabase.CreateAsset(config, CONFIG_PATH);

        EnsureSceneExists(config.startupSceneName, ref config.startupScene);
        EnsureSceneExists(config.loadingSceneName, ref config.loadingScene);

        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssets();
        Debug.LogWarning("[RKode] ProjectConfig created with defaults. Configure it in Project Settings > RKode.");

        return config;
    }

    private static void EnsureSceneExists(string sceneName, ref SceneAsset sceneAsset) {
        var path = $"Assets/Scenes/{sceneName}.unity";
        
        if (!System.IO.File.Exists(path)) {
            System.IO.Directory.CreateDirectory("Assets/Scenes");
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            EditorSceneManager.SaveScene(scene, path);
            EditorSceneManager.CloseScene(scene, true);
            Debug.Log($"[RKode] Created default scene at '{path}'.");
        }

        AssetDatabase.Refresh();
        sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
        EditorUtility.SetDirty(sceneAsset);
    }

    internal static void ResetConfig() {
        var config = Resources.Load<ProjectConfig>(RESOURCES_PATH);
        if (config != null) {
            AssetDatabase.DeleteAsset(CONFIG_PATH);
            AssetDatabase.Refresh();
        }

        EnsureConfigExists();
        Debug.Log("[RKode] Project settings reset to defaults.");
    }
}
}
#endif