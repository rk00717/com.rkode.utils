#if UNITY_EDITOR
using UnityEditor;
using RKode.Editor.ProjectSettings;
using UnityEngine;

namespace RKode.Core.Editor {
internal class SceneLoaderSettingsSection : ISettingsSection {
    public string Title => "Scene Loader";

    public void Draw(SerializedObject so) {
        EditorGUILayout.PropertyField(so.FindProperty("loadingScene"), new GUIContent("Loading Scene"));
    }
}
}
#endif