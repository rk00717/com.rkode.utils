#if UNITY_EDITOR
using UnityEditor;

namespace RKode.Startup {
internal static class StartupMenu {
    [MenuItem("RKode/Startup/Enable Startup Redirect", priority = -10)]
    private static void Toggle() {
        bool confirm = EditorUtility.DisplayDialog(
            "Startup Redirect",
            "Load startup scene on Play?",
            "Enable",
            "Disable"
        );

        Bootstrap.SetEnabled(confirm);
    }
}
}
#endif