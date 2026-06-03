#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace RKode.Utils.Editor {
public static class PaletteButtonStyle {
    private static readonly Dictionary<Color, Texture2D> _texCache = new();
    private static readonly Dictionary<Color, Texture2D> _borderTexCache = new();

    public static GUIStyle Create(Color color, bool isSelected, int height = 26, int fontSize = 11) {
        var hoverColor  = AdjustBrightness(color, 1.15f);
        var activeColor = AdjustBrightness(color, 0.82f);

        var borderColor = AdjustBrightness(color, 0.45f); 
        borderColor.a = 1f;
        var hoverBorderColor = AdjustBrightness(hoverColor, 0.45f); 
        hoverBorderColor.a = 1f;
        var activeBorderColor = AdjustBrightness(activeColor, 0.45f); 
        activeBorderColor.a = 1f;

        var style = new GUIStyle {
            fixedHeight = height,
            fontSize = fontSize,
            fontStyle = isSelected ? FontStyle.Bold : FontStyle.Normal,
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(10, 10, 0, 0),
            margin = new RectOffset(0, 0, 1, 1),
            border = new RectOffset(2, 2, 2, 2),

            normal = { 
                background = MakeBorderedTex(color, borderColor), 
                textColor = GetSoftTextColor(color)
            },

            hover = { 
                background = MakeBorderedTex(hoverColor, hoverBorderColor), 
                textColor = GetSoftTextColor(hoverColor)
            },

            active = { 
                background = MakeBorderedTex(activeColor, activeBorderColor), 
                textColor = GetSoftTextColor(activeColor)
            },

            focused = { 
                background = MakeBorderedTex(color, borderColor), 
                textColor = GetSoftTextColor(color)
            },

            onNormal = { 
                background = MakeBorderedTex(hoverColor, hoverBorderColor), 
                textColor = GetSoftTextColor(hoverColor)
            },
        };

        return style;
    }

    public static string FormatLabel(string shortLabel, string fullLabel, bool isSelected) =>
        isSelected? $"▶  {fullLabel} ({shortLabel})" : $"    {fullLabel} ({shortLabel})";

    public static Color GetSoftTextColor(Color bg) {
        float luminance = GetLuminance(bg);

        if (luminance > 0.179f) {
            // Dark text on light background — tinted with bg hue slightly
            return new Color(
                Mathf.Lerp(0.08f, 0.18f, bg.r),
                Mathf.Lerp(0.08f, 0.18f, bg.g),
                Mathf.Lerp(0.08f, 0.18f, bg.b),
                1f);
        } else {
            // Light text on dark background — warm white tinted with bg hue
            return new Color(
                Mathf.Lerp(0.88f, 1f, bg.r * 0.3f),
                Mathf.Lerp(0.88f, 1f, bg.g * 0.3f),
                Mathf.Lerp(0.88f, 1f, bg.b * 0.3f),
                1f);
        }
    }

    public static Color AdjustBrightness(Color color, float factor) {
        var adjusted = new Color(
            Mathf.Clamp01(color.r * factor),
            Mathf.Clamp01(color.g * factor),
            Mathf.Clamp01(color.b * factor),
            color.a);
        return adjusted;
    }

    public static float GetLuminance(Color c) {
        float r = c.r <= 0.03928f ? c.r / 12.92f : Mathf.Pow((c.r + 0.055f) / 1.055f, 2.4f);
        float g = c.g <= 0.03928f ? c.g / 12.92f : Mathf.Pow((c.g + 0.055f) / 1.055f, 2.4f);
        float b = c.b <= 0.03928f ? c.b / 12.92f : Mathf.Pow((c.b + 0.055f) / 1.055f, 2.4f);
        return 0.2126f * r + 0.7152f * g + 0.0722f * b;
    }

    public static Texture2D MakeTex(Color color) {
        if (_texCache.TryGetValue(color, out var cached) && cached != null)
            return cached;

        var tex = new Texture2D(4, 4);
        var pixels = new Color[16];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;

        tex.SetPixels(pixels);
        tex.Apply();
        tex.hideFlags = HideFlags.DontSave;

        _texCache[color] = tex;
        return tex;
    }

    public static Texture2D MakeBorderedTex(Color fill, Color border) {
        var cacheKey = new Color(
            fill.r * 0.5f + border.r * 0.5f,
            fill.g * 0.5f + border.g * 0.5f,
            fill.b * 0.5f + border.b * 0.5f,
            fill.a
        );

        if (_borderTexCache.TryGetValue(cacheKey, out var cached) && cached != null)
            return cached;

        int w = 4;
        int h = 8;
        var tex = new Texture2D(w, h);
        var pixels = new Color[w * h];

        var topColor = AdjustBrightness(fill, 1.12f);
        var bottomColor = AdjustBrightness(fill, 0.88f);

        for (int y = 0; y < h; y++) {
            for (int x = 0; x < w; x++) {
                bool isBorder = x == 0 || x == w - 1 || y == 0 || y == h - 1;

                if (isBorder) {
                    pixels[y * w + x] = border;
                } else {
                    // Lerp from bottom to top
                    float t = (float)y / (h - 1);
                    pixels[y * w + x] = Color.Lerp(bottomColor, topColor, t);
                }
            }
        }

        tex.SetPixels(pixels);
        tex.filterMode = FilterMode.Point;
        tex.Apply();
        tex.hideFlags = HideFlags.DontSave;

        _borderTexCache[cacheKey] = tex;
        return tex;
    }

    public static Texture2D MakeBorderedTex(Color fill, Color border, RectOffset thickness = default) {
        var cacheKey = new Color(
            fill.r * 0.5f + border.r * 0.5f,
            fill.g * 0.5f + border.g * 0.5f,
            fill.b * 0.5f + border.b * 0.5f,
            fill.a
        );

        if (_borderTexCache.TryGetValue(cacheKey, out var cached) && cached != null)
            return cached;

        int w = 4;
        int h = 8;
        var tex = new Texture2D(w, h);
        var pixels = new Color[w * h];

        var topColor = AdjustBrightness(fill, 1.12f);
        var bottomColor = AdjustBrightness(fill, 0.88f);

        for (int y = 0; y < h; y++) {
            for (int x = 0; x < w; x++) {
                bool isBorder = x == thickness.left || x == w - thickness.right || y == thickness.bottom || y == h - thickness.top;

                if (isBorder) {
                    pixels[y * w + x] = border;
                } else {
                    // Lerp from bottom to top
                    float t = (float)(y - thickness.bottom) / (h - 1 - thickness.top - thickness.bottom);
                    pixels[y * w + x] = Color.Lerp(bottomColor, topColor, Mathf.Clamp01(t));
                }
            }
        }

        tex.SetPixels(pixels);
        tex.filterMode = FilterMode.Point;
        tex.Apply();
        tex.hideFlags = HideFlags.DontSave;

        _borderTexCache[cacheKey] = tex;
        return tex;
    }

    public static void ClearCache() {
        foreach (var tex in _texCache.Values)
            if (tex != null) Object.DestroyImmediate(tex);
        _texCache.Clear();

        foreach (var tex in _borderTexCache.Values)
            if (tex != null) Object.DestroyImmediate(tex);
        _borderTexCache.Clear();
    }
}
}
#endif