#if UNITY_EDITOR
using UnityEditor;

namespace RKode.Editor.ProjectSettings {
internal interface ISettingsSection {
    string Title { get; }
    void Draw(SerializedObject so);
}
}
#endif