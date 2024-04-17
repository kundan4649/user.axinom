using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using z5.ms.common.abstractions;

namespace z5.ms.common.extensions
{
    /// <summary>JSON serialization and formatting extensions</summary>
    public static class JsonExtensions
    {
        /// <summary>Deserialize stream into object of type T</summary>
        /// <typeparam name="T">Target object type</typeparam>
        /// <param name="s">Stream</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>object of type T</returns>
        public static T TryDeserialize<T>(this Stream s, ILogger logger = null) where T : class, new()
        {
            try
            {
                return s.Deserialize<T>();
            }
            catch (Exception ex)
            {
                logger?.LogError($"Failed to deserialize stream to {typeof(T).Name} - {ex.Message}");
                return null;
            }
        }

        /// <summary>Deserialize json string into object of type T</summary>
        /// <typeparam name="T">Target object type</typeparam>
        /// <param name="jsonString">string</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>object of type T</returns>
        public static T TryDeserialize<T>(this string jsonString, ILogger logger = null) where T : class, new()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception ex)
            {
                logger?.LogError($"Failed to deserialize string to {typeof(T).Name}. {ex.Message}");
                return null;
            }
        }

        /// <summary>Deserialize json object into object of type T</summary>
        /// <typeparam name="T">Target object type</typeparam>
        /// <param name="jsonObject">JObject</param>
        /// <returns>object of type T</returns>
        public static Result<T> Deserialize<T>(this JObject jsonObject) where T : class, new()
        {
            try
            {
                return Result<T>.FromValue(jsonObject.ToObject<T>());
            }
            catch (Exception ex)
            {
                return Result<T>.FromError(1, ex.Message);
            }
        }

        /// <summary>Deserialize stream into object of type T</summary>
        /// <typeparam name="T">Target object type</typeparam>
        /// <param name="s">Stream</param>
        /// <returns>object of type T</returns>
        public static T Deserialize<T>(this Stream s)
        {
            using (var reader = new StreamReader(s))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var ser = new JsonSerializer();
                return ser.Deserialize<T>(jsonReader);
            }
        }

        /// <summary>Deserialize stream into list of elements of type T</summary>
        /// <typeparam name="T">Target list elements type</typeparam>
        /// <param name="s">Stream</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>List of objects of type T or null</returns>
        public static List<T> TryDeserializeList<T>(this Stream s, ILogger logger = null)
        {
            try
            {
                return s.Deserialize<List<T>>();
            }
            catch (Exception ex)
            {
                logger?.LogError($"Failed to deserialize stream to {typeof(T).Name} - {ex.Message}");
                return null;
            }
        }

        /// <summary>Deserialize JSON string into list of elements of type T</summary>
        /// <typeparam name="T">Target list elements type</typeparam>
        /// <param name="jsonString">Valid JSON string</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>List of objects of type T or null</returns>
        public static List<T> TryDeserializeList<T>(this string jsonString, ILogger logger = null)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<T>>(jsonString);
            }
            catch (Exception ex)
            {
                logger?.LogError($"Failed to deserialize string to {typeof(T).Name}. {ex.Message}");
                return null;
            }
        }

        /// <summary>Serialize object as T</summary>
        /// <typeparam name="T">input object type</typeparam>
        /// <param name="obj">input object</param>
        /// <returns>Serialized JSON string</returns>
        public static string Serialize<T>(this T obj) => JsonConvert.SerializeObject(obj);

        /// <summary>Serializes an object, but also formats the resulting JSON for readability</summary>
        /// <typeparam name="T">input object type</typeparam>
        /// <param name="obj">input object</param>
        /// <returns>Serialized JSON string</returns>
        public static string SerializeFormatted<T>(this T obj) => JsonConvert.SerializeObject(obj, Formatting.Indented);

        /// <summary>Convert collection of key-value pairs to dictionary</summary>
        /// <typeparam name="T">key and value param type</typeparam>
        /// <param name="enumerable">source collection</param>
        /// <returns>dictionary</returns>
        public static IDictionary<T, T> ToDictionary<T>(this IEnumerable<KeyValuePair<T, T>> enumerable)
            => enumerable.ToDictionary(k => k.Key, v => v.Value);

        /// <summary>Pretty-print specified object</summary>
        public static string PrettyPrint(this object value)
            => JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new JsonConverter[]{ new StringEnumConverter() }
            });
    }
}