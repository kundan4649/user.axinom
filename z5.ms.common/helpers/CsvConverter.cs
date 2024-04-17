using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace z5.ms.common.helpers
{
    /// <summary>JSON CSV converter</summary>
    /// <remarks>Converts comma-separated string of values to List of strings and back</remarks>
    public class CsvConverter : JsonConverter
    {
        /// <inheritdoc />
        public override bool CanConvert(Type objectType) => true;

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String || reader.Value == null)
                return new List<string>();

            var str = reader.Value.ToString();
            return String.IsNullOrEmpty(str) ? new List<string>() : new List<string>(str.Split(','));
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vals = value as IEnumerable<string>;
            if (vals == null)
                throw new InvalidCastException(
                    "Unable to parse object to IEnumerable<string>. Object is not a valid string collection");

            writer.WriteValue(string.Join(",", vals));
        }
    }

    /// <summary>Extensions methods for splitting CSV string</summary>
    public static class CsvExtensions
    {
        /// <summary>Split csv string and trim leading and tailing whitespace</summary>
        private static IEnumerable<string> SplitCsvFunc(this string csvString)
        {
            if(String.IsNullOrWhiteSpace(csvString))
                yield break;

            foreach (var item in csvString.Split(','))
                if (!String.IsNullOrWhiteSpace(item))
                    yield return item.Trim();
        }
        
        /// <summary>Split csv string and trim leading and tailing whitespace</summary>
        public static List<string> SplitCsv(this string csvString) => SplitCsvFunc(csvString).ToList();
    }
}