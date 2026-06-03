using UnityEngine.SceneManagement;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

namespace RKode.Startup {
public class Bootstrap {
#if UNITY_EDITOR
    private static bool _isRedirecting;

    [InitializeOnLoadMethod]
    private static void RegisterPlayModeHook() {
        if (EditorPrefs.GetBool(StartupConstants.CAN_LOAD_KEY, false))
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    public static void SetEnabled(bool enabled) {
        EditorPrefs.SetBool(StartupConstants.CAN_LOAD_KEY, enabled);
        if (enabled) {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        } else {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }
    }
    
    internal static void OnPlayModeStateChanged(PlayModeStateChange state) {
        if (state == PlayModeStateChange.ExitingEditMode) {
            var config = GetConfig();
            if (config.startupScene == null)
                Debug.LogWarning($"[RKode.Startup] No startup scene assigned. Using default '{config.startupSceneName}'. Assign it in Project Settings > RKode > Startup.");
            
            if (config.loadingScene == null)
                Debug.LogWarning($"[RKode.Startup] No loading scene assigned. Using default '{config.loadingSceneName}'. Assign it in Project Settings > RKode > Startup.");

            if (_isRedirecting) 
                return;

            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.name == config?.startupSceneName) 
                return;

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                EditorApplication.isPlaying = false;
                return;
            }

            SessionState.SetString(StartupConstants.PREVIOUS_SCENE_KEY, activeScene.path);
            _isRedirecting = true;
            EditorApplication.isPlaying = false;
            EditorApplication.update += OpenBootstrapThenPlay;
        }

        if (state == PlayModeStateChange.EnteredPlayMode) {
            _isRedirecting = false;
        }

        if (state == PlayModeStateChange.EnteredEditMode) {
            var previousPath = SessionState.GetString(StartupConstants.PREVIOUS_SCENE_KEY, "");
            if (string.IsNullOrEmpty(previousPath)) 
                return;

            SessionState.EraseString(StartupConstants.PREVIOUS_SCENE_KEY);
            SessionState.EraseBool(StartupConstants.REDIRECT_COMPLETED_KEY);
            _isRedirecting = true;
            EditorSceneManager.OpenScene(previousPath);
            _isRedirecting = false;
        }
    }

    private static void OpenBootstrapThenPlay() {
        EditorApplication.update -= OpenBootstrapThenPlay;

        var config = GetConfig();
        if (config == null) {
            Debug.LogError("[RKode.Startup] StartupConfig not found. Make sure it exists in Resources/RKode/.");
            _isRedirecting = false;
            return;
        }

        var bootstrapPath = FindScenePath(config.startupSceneName);
        if (string.IsNullOrEmpty(bootstrapPath)) {
            Debug.LogError($"[RKode.Startup] Could not find scene '{config.startupSceneName}'. Make sure it exists in Build Settings.");
            _isRedirecting = false;
            return;
        }

        SessionState.SetBool(StartupConstants.REDIRECT_COMPLETED_KEY, true);
        EditorSceneManager.OpenScene(bootstrapPath);
        EditorApplication.isPlaying = true;
    }

    private static string FindScenePath(string sceneName) {
        foreach (var scene in EditorBuildSettings.scenes) {
            if (System.IO.Path.GetFileNameWithoutExtension(scene.path) == sceneName)
                return scene.path;
        }

        var guids = AssetDatabase.FindAssets($"{sceneName} t:Scene");
        foreach (var guid in guids) {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (System.IO.Path.GetFileNameWithoutExtension(path) == sceneName)
                return path;
        }

        return null;
    }

#endif

#if !UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBuildSceneLoad() {
        var config = GetConfig();
        if (config == null) {
            Debug.LogWarning("[RKode.Startup] StartupConfig not found in Resources. Skipping startup redirect.");
            return;
        }

        if (SceneManager.GetActiveScene().name != config.startupScene)
            SceneManager.LoadSceneAsync(config.startupScene);
    }
#endif

    private static ProjectConfig GetConfig() =>
        Resources.Load<ProjectConfig>(StartupConstants.CONFIG_PATH);
}
}