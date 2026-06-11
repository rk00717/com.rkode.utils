namespace RKode.Utils {
public static class ProjectScopedKey {
    private static string _key;
    public static string Key {
        get {
            if(_key != null) return _key;

#if UNITY_EDITOR
            _key = UnityEditor.PlayerSettings.productGUID.ToString();
#else
            _key = $"{UnityEngine.Application.companyName}.{UnityEngine.Application.productName}";
#endif
            return _key;
        }
    }

    public static string Get(string baseKey) => $"{baseKey}.{Key}";
}
}