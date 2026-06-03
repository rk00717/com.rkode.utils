using UnityEngine;

namespace RKode.Utils {
public static class ColorUtility {
    public static string ColorToHex(this Color color) {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");

        return hex;
    }

    public static Color HexToColor(string hex) {
        hex = hex.Replace("0x", "");
        hex = hex.Replace("#", "");

        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte a = hex.Length >= 8 
            ? byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) 
            : (byte)255;

        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }

    public static Color WithAlpha(Color c, float a) => new Color(c.r, c.g, c.b, a);
}
}