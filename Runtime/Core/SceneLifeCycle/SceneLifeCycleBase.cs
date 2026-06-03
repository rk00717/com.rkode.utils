using UnityEngine;

namespace RKode.Utils {
public abstract class SceneLifeCycleBase : MonoBehaviour {
    protected abstract string SceneName { get; }
    [SerializeField] protected bool isAdditive = false;
    public System.Action onComplete;

    public void ShowScene() {
        if(isAdditive)
            SceneLoader.Instance.LoadAdditiveScene(SceneName);
        else
            SceneLoader.Instance.LoadScene(SceneName, onComplete);
            
    }

    public void HideScene() {
        SceneLoader.Instance.UnloadScene(SceneName);
    }
}
}