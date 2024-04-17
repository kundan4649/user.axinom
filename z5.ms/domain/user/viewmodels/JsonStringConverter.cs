using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace z5.ms.domain.user.viewmodels
{
    public class JsonStringConverter : JsonConverter<string>
    {
        string readerValueString = null;

        private static string pattern = @"\t|\n|\r|\\n|\\r|\\t";
        public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            readerValueString = Convert.ToString(reader.Value);

            if (!string.IsNullOrEmpty(readerValueString))
            {
                if (reader.Value.Equals(true) || reader.Value.Equals(false))
                {
                    //conversion added to handle capitalization of true/false values sent as object and not as string.
                    return readerValueString.ToLower();
                }
                else
                {
                    return readerValueString;
                }
            }
            else
            {
                return readerValueString;
            }
        }
        public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
        {
            value = (value == null ? string.Empty : value);
            writer.WriteValue(Regex.Replace(value, pattern, string.Empty));
        }
    }
}
