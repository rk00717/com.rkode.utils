using UnityEngine;

namespace RKode.Utils {
public class QuitApplication : MonoBehaviour {
    public void Quit(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
}