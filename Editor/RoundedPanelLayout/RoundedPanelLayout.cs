#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RKode.Utils.Editor {
public static class RoundedPanelLayout {
    private static readonly Stack<RoundedPanelStyle> _styleStack = new();
    private static readonly Stack<Rect> _rectStack = new();

    public static void BeginRoundedBox(RoundedPanelStyle style) {
        _styleStack.Push(style);

        Rect rect;
        if(style == null) {
            rect = EditorGUILayout.BeginVertical();
            return;
        }

        rect = EditorGUILayout.BeginVertical(style.style, GUILayout.ExpandWidth(true));
        if(Event.current.type == EventType.Repaint && rect.width > 1) {
            DrawRoundedBox(rect, style);
        }

        _rectStack.Push(rect);
    }

    public static void EndRoundedBox() {
        if (_styleStack.Count == 0) {
            Debug.LogWarning("RoundedVertical.End() called without matching Begin().");
            GUILayout.EndVertical();
            return;
        }

        _styleStack.Pop();
        _rectStack.Pop();
        EditorGUILayout.EndVertical();
    }

    private static void DrawRoundedBox(Rect rect, RoundedPanelStyle style) {
        float radius = Mathf.Clamp(style.radius, 0f, Mathf.Min(rect.width, rect.height) / 2f);

        Handles.BeginGUI();
        Color prev = Handles.color;

        // Draw rounded fill
        if (style.fillColor.a > 0f) {
            Handles.color = style.fillColor;
            Handles.DrawAAConvexPolygon(BuildRoundedRectPath(rect, radius, style.cornerSegments));
        }

        // Draw rounded border
        if (style.borderWidth > 0f){
            Handles.color = style.borderColor;
            Handles.DrawAAPolyLine(style.borderWidth, BuildRoundedRectOutline(rect, radius, style.cornerSegments));
        }

        Handles.color = prev;
        Handles.EndGUI();
    }

    private static Vector3[] BuildRoundedRectOutline(Rect rect, float radius, int cornerSegments) {
        var pts = new List<Vector3>();
        Vector2[] centers = {
            new(rect.xMax - radius, rect.yMin + radius), // top-right
            new(rect.xMax - radius, rect.yMax - radius), // bottom-right
            new(rect.xMin + radius, rect.yMax - radius), // bottom-left
            new(rect.xMin + radius, rect.yMin + radius), // top-left
        };

        float[] startAngles = { 270f, 0f, 90f, 180f };

        for (int i = 0; i < 4; i++) {
            float start = startAngles[i];
            for (int seg = 0; seg <= cornerSegments; seg++) {
                float a = (start + seg * 90f / cornerSegments) * Mathf.Deg2Rad;
                pts.Add(new Vector3(
                    centers[i].x + Mathf.Cos(a) * radius,
                    centers[i].y + Mathf.Sin(a) * radius
                ));
            }
        }

        pts.Add(pts[0]);
        return pts.ToArray();
    }

    private static Vector3[] BuildRoundedRectPath(Rect rect, float radius, int cornerSegments) => 
        BuildRoundedRectOutline(rect, radius, cornerSegments);

    public static void DrawRoundedRect(Rect rect, RoundedPanelStyle style, bool topLeft = true, bool topRight = true, bool bottomRight = true, bool bottomLeft = true) {
        if (Event.current.type != EventType.Repaint) 
            return;

        float radius = Mathf.Clamp(style.radius, 0f, Mathf.Min(rect.width, rect.height) / 2f);

        Handles.BeginGUI();
        Color prev = Handles.color;

        var path = BuildPartialRoundedRect(rect, radius, style.cornerSegments, topLeft, topRight, bottomRight, bottomLeft);

        if (style.fillColor.a > 0f) {
            Handles.color = style.fillColor;
            Handles.DrawAAConvexPolygon(path);
        }

        if (style.borderWidth > 0f) {
            Handles.color = style.borderColor;
            Handles.DrawAAPolyLine(style.borderWidth, path);
        }

        Handles.color = prev;
        Handles.EndGUI();
    }

    private static Vector3[] BuildPartialRoundedRect(Rect rect, float radius, int segs, bool tl, bool tr, bool br, bool bl) {
        var pts = new List<Vector3>();

        // top-right
        if (tr) {
            Vector2 c = new(rect.xMax - radius, rect.yMin + radius);
            for (int s = 0; s <= segs; s++) {
                float a = (270f + s * 90f / segs) * Mathf.Deg2Rad;
                pts.Add(new Vector3(c.x + Mathf.Cos(a) * radius, c.y + Mathf.Sin(a) * radius));
            }
        } else {
            pts.Add(new Vector3(rect.xMax, rect.yMin));
            pts.Add(new Vector3(rect.xMax, rect.yMin));
        }

        // bottom-right
        if (br) {
            Vector2 c = new(rect.xMax - radius, rect.yMax - radius);
            for (int s = 0; s <= segs; s++) {
                float a = (0f + s * 90f / segs) * Mathf.Deg2Rad;
                pts.Add(new Vector3(c.x + Mathf.Cos(a) * radius, c.y + Mathf.Sin(a) * radius));
            }
        } else {
            pts.Add(new Vector3(rect.xMax, rect.yMax));
            pts.Add(new Vector3(rect.xMax, rect.yMax));
        }

        // bottom-left
        if (bl) {
            Vector2 c = new(rect.xMin + radius, rect.yMax - radius);
            for (int s = 0; s <= segs; s++) {
                float a = (90f + s * 90f / segs) * Mathf.Deg2Rad;
                pts.Add(new Vector3(c.x + Mathf.Cos(a) * radius, c.y + Mathf.Sin(a) * radius));
            }
        } else {
            pts.Add(new Vector3(rect.xMin, rect.yMax));
            pts.Add(new Vector3(rect.xMin, rect.yMax));
        }

        // top-left
        if (tl) {
            Vector2 c = new(rect.xMin + radius, rect.yMin + radius);
            for (int s = 0; s <= segs; s++) {
                float a = (180f + s * 90f / segs) * Mathf.Deg2Rad;
                pts.Add(new Vector3(c.x + Mathf.Cos(a) * radius, c.y + Mathf.Sin(a) * radius));
            }
        } else {
            pts.Add(new Vector3(rect.xMin, rect.yMin));
            pts.Add(new Vector3(rect.xMin, rect.yMin));
        }

        pts.Add(pts[0]); // close
        return pts.ToArray();
    }
}
}
#endif