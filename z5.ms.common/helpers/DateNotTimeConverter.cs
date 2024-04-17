using Newtonsoft.Json.Converters;

namespace z5.ms.common.helpers
{
    /// <summary>
    /// Serializes a DateTime without the time component
    /// </summary>
    public class DateNotTimeConverter : IsoDateTimeConverter
    {
        /// <inheritdoc />
        public DateNotTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
