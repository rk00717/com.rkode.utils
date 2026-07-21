using UnityEngine;
using Object = UnityEngine.Object;

namespace RKode {
public static class Logger {
	public static bool IsLoggingEnabled { get; set; } = true;
	
    public static string Color(this string myStr, string color) {
        return $"<color={color}>{myStr}</color>";
    }

    private static void DoLog(System.Action<string, Object> LogFunction, string prefix, Object myObj, bool condition = true, params object[] msg) {
#if UNITY_EDITOR
		if(!IsLoggingEnabled || !condition) return;

        var name = (myObj? $"{myObj.GetType()} | {myObj.name}" : "NullObject").Color("lightblue"); 
        LogFunction($"{prefix} [{name}]: {System.String.Join("; ", msg)}\n ", myObj);
#endif
    }

#region Standard Logs

    public static void Log(this Object myObj, params object[] msg) {
        DoLog(Debug.Log, "◉", myObj, msg:msg);
    }

    public static void LogError(this Object myObj, params object[] msg) {
        DoLog(Debug.LogError, "◉".Color("red"), myObj, msg:msg);    // ✗
    }

    public static void LogWarning(this Object myObj, params object[] msg) {
        DoLog(Debug.LogWarning, "◉".Color("yellow"), myObj, msg:msg);   // ⚠
    }

    public static void LogSuccess(this Object myObj, params object[] msg) {
        DoLog(Debug.Log, "◉".Color("green"), myObj, msg:msg);   // ✓
    }
    
#endregion
    
#region Conditional Logs

public static void Log(this Object myObj, bool condition, params object[] msg) {
        DoLog(Debug.Log, "◉", myObj, condition, msg);
    }

    public static void LogError(this Object myObj, bool condition, params object[] msg) {
        DoLog(Debug.LogError, "◉".Color("red"), myObj, condition, msg);    // ✗
    }

    public static void LogWarning(this Object myObj, bool condition, params object[] msg) {
        DoLog(Debug.LogWarning, "◉".Color("yellow"), myObj, condition, msg);   // ⚠
    }

    public static void LogSuccess(this Object myObj, bool condition, params object[] msg) {
        DoLog(Debug.Log, "◉".Color("green"), myObj, condition, msg);   // ✓
    }

#endregion
}
}