using Newtonsoft.Json;

namespace RKode.Utils.Data {
public class UniversalJsonParser : IUniversalParser {
    public T OnParse<T>(string rawData) {
        try {
            // NewtonSoft handles nested objects, lists, and dictionaries automatically
            return JsonConvert.DeserializeObject<T>(rawData);
        } catch (System.Exception e) {
            UnityEngine.Debug.LogError($"[Parser] Failed to parse {typeof(T)}: {e.Message}");
            return default;
        }
    }
}
}