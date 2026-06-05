using UnityEngine;

namespace RKode.Utils {
public class QuitApplication : MonoBehaviour {
    public void Quit() => QuitApplication.Execute();
    
    public static void Execute(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
}