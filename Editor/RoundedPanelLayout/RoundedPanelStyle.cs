#if UNITY_EDITOR
using UnityEngine;

namespace RKode.Utils.Editor {
public class RoundedPanelStyle {
    public GUIStyle _style { get; private set; }

    public float radius = 8f;
    public Color fillColor = ColorUtility.HexToColor("#0000001F");
    public Color borderColor = ColorUtility.HexToColor("#ffffffff");    // ColorUtility.HexToColor("#EB7D7DFF")
    public float borderWidth = 2f;
    public int cornerSegments = 8;

    public GUIStyle style => _style ??= BuildStyle();

    public RoundedPanelStyle() { }

    public RoundedPanelStyle(GUIStyle baseStyle) {
        _style = new GUIStyle(baseStyle ?? GUIStyle.none);
    }

    private GUIStyle BuildStyle() {
        return new GUIStyle(GUIStyle.none) {
            margin  = new RectOffset(4, 4, 4, 4),
            padding = new RectOffset(10, 10, 10, 10)
        };
    }
}
}
#endif