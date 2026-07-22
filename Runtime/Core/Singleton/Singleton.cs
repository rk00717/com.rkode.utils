using System.Reflection;
using UnityEngine;

namespace RKode.Utils {
/// <summary>
/// Generic singleton base class for MonoBehaviours.
/// Ensures only one instance exists and optionally persists across scenes.
/// 
/// Usage:
/// public class MyManager : Singleton<MyManager>
/// {
///     protected override void OnAwake()
///     {
///         base.OnAwake();
///         // Your initialization here
///     }
/// }
/// </summary>
/// <typeparam name="T">The type of the singleton class</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting = false;

    [Tooltip("Should this singleton persist across scene loads?")]
    [SerializeField] protected bool dontDestroyOnLoad = true;

    private static readonly bool _autoCreate =
        typeof(T).GetCustomAttribute<SingletonConfigAttribute>()?.AutoCreate ?? false;

    /// <summary>
    /// Gets the singleton instance. Returns null if none exists.
    /// </summary>
    public static T Instance {
        get {
            if (_applicationIsQuitting) {
                Logger.LogWarning(_instance, $"[Singleton] Instance of {typeof(T)} already destroyed on application quit. Returning null.");
                return null;
            }

            lock (_lock) {
                if(_instance != null) 
                    return _instance;

                _instance = FindObjectOfType<T>();
                if (_instance == null) { 
                    if(!_autoCreate) {
                        Logger.LogWarning(_instance, $"[Singleton] No instance of {typeof(T)} found in scene. Please add one to the scene.");
                        return null;
                    }

                    var go = new GameObject($"{typeof(T).Name} (Auto)");
                    _instance = go.AddComponent<T>();
                }

                return _instance;
            }
        }
    }

    /// <summary>
    /// Checks if an instance exists without creating one or logging warnings.
    /// Useful for cleanup code in OnDestroy methods.
    /// </summary>
    public static bool HasInstance => _instance != null && !_applicationIsQuitting;

    /// <summary>
    /// Resets the singleton instance
    /// </summary>
    public static void SetInstanceNull() { 
        _instance = null;
        _applicationIsQuitting = false;
    }

    /// <summary>
    /// Unity Awake - handles singleton logic. Override OnAwake() in derived classes instead.
    /// </summary>
    protected virtual void Awake() {
        _applicationIsQuitting = false;
        CreateSingleton();
    }

    /// <summary>
    /// Cleanup when singleton is destroyed
    /// </summary>
    protected virtual void OnDestroy() {
        if(_instance != this) return;

        this.Log($"[Singleton] {typeof(T).Name} destroyed.");
        _instance = null;
    }

    /// <summary>
    /// Handle application quit to prevent object access errors
    /// </summary>
    protected virtual void OnApplicationQuit() {
        _applicationIsQuitting = true;
    }

    /// <summary>
    /// Handling Singleton Creation
    /// </summary>
    private void CreateSingleton() {
        if (_instance == null) { 
            _instance = this as T;
            
            if (dontDestroyOnLoad) {
                if(transform.parent != null) {
                    this.Log($"Detaching {typeof(T)} from its parent as its a singleton");
                    transform.SetParent(null);
                }

                DontDestroyOnLoad(this);
            }

            OnAwake();
        } else if (_instance != this) { 
            this.LogWarning($"[Singleton] Duplicate instance of {typeof(T).Name} detected on {gameObject.name}. Destroying duplicate.");
            Destroy(this);
        }
    }

    /// <summary>
    /// Handle Singleton's Clean up..
    /// </summary>
    protected void ClearInstance() {
        if(_instance == null) {
            Logger.Log(null, $"[Singleton] {typeof(T).Name} Instance doesnt exists. (returning)...");
            return;
        }

        this.LogSuccess($"[Singleton] {typeof(T).Name} Instance Destroyed.");

        SetInstanceNull();
        Destroy(this);
    }

    /// <summary>
    /// Called after singleton is initialized. Override this instead of Awake() in derived classes.
    /// </summary>
    protected virtual void OnAwake() { }
}
}
