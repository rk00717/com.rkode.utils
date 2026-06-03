#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using RKode.Editor.ProjectSettings;

namespace RKode.Startup.Editor {
internal class StartupSettingsSection : ISettingsSection {
    public string Title => "Startup";

    public void Draw(SerializedObject so) {
        var canLoadBootstrap = EditorPrefs.GetBool(StartupConstants.CAN_LOAD_KEY, false);
        Bootstrap.SetEnabled(EditorGUILayout.Toggle("Enable Startup Redirect", canLoadBootstrap));
        EditorGUILayout.PropertyField(so.FindProperty("startupScene"), new GUIContent("Startup Scene"));
    }
}
}
#endif