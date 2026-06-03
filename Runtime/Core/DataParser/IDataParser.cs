namespace RKode.Utils.Data {
public interface IUniversalParser {
    /// <summary>
    /// Converts a raw string into a specified Type T.
    /// </summary>
    T OnParse<T>(string rawData);
}
}