#if UNITY_EDITOR
using UnityEngine;

using UnityEditor;

using System.Collections.Generic;
using System.Linq;

namespace RKode.Editor.ProjectSettings {
internal static class SettingsProvider {
    [SettingsProvider]
    public static UnityEditor.SettingsProvider CreateSettingsProvider() {
        return new UnityEditor.SettingsProvider("Project/RKode", SettingsScope.Project) {
            label = "RKode - Settings",
            guiHandler = (searchCxt) => {
                BuildUI();
            },
            keywords = new HashSet<string> { "rkode", "startup", "scene", "loading", "bootstrap" }
        };
    }

    private static void BuildUI() {
        var config = SettingsRegister.EnsureConfigExists();
        var so = new SerializedObject(config);

        var previousLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 260;
        EditorGUILayout.Space(5);

        foreach (var section in GetSections()) {
            EditorGUILayout.LabelField(section.Title, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            section.Draw(so);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Reset to Defaults", GUILayout.Width(150))) {
            if (EditorUtility.DisplayDialog(
                "Reset RKode Project Settings",
                "This will reset all settings to defaults and recreate default scenes if missing. Are you sure?",
                "Reset",
                "Cancel")) {
                SettingsRegister.ResetConfig();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUIUtility.labelWidth = previousLabelWidth;

        so.ApplyModifiedProperties();
    }

    private static IEnumerable<ISettingsSection> GetSections() {
        return typeof(ISettingsSection).Assembly
            .GetTypes()
            .Where(t => typeof(ISettingsSection).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (ISettingsSection)System.Activator.CreateInstance(t));
    }
}
}
#endif