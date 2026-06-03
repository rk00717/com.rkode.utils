using System.IO;
using UnityEngine;

namespace RKode.Utils {
public static class SaveDataHelper {
    private static string PATH = Application.persistentDataPath + "/";
    private static string EXT = ".rkode";

    public static void SaveData<T>(T data, string fileName){
        var convertedData = JsonUtility.ToJson(data, true);
        File.WriteAllText(PATH + fileName + EXT, convertedData);

        Debug.Log($"[SaveDataHelper] Saving to: {Application.persistentDataPath}/{fileName}.rkode");
    }

    public static T LoadData<T>(string fileName){
        var path = PATH + fileName + EXT;
        if(!File.Exists(path))
            return default;

        var convertedData = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(convertedData);
    }
}
}