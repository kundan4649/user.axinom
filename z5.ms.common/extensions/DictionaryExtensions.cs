using System.Collections.Generic;

namespace z5.ms.common.extensions
{
    /// <summary>
    /// Extensions for dictionary types
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>Query a dictionary, returning null if key is not found</summary>
        public static T2? GetNullableValue<T, T2>(this IDictionary<T, T2> dict, T key) where T2 : struct 
            => dict.TryGetValue(key, out var result) ? (T2?)result : null;

        /// <summary>Query a dictionary, returning null if key is not found</summary>
        public static string GetValueOrNull(this IDictionary<string, string> dict, string key)
            => dict.TryGetValue(key, out var result) ? result : null;
    }
}
