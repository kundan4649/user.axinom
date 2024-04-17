using Newtonsoft.Json.Converters;

namespace z5.ms.common.helpers
{
    /// <summary>
    /// Serializes a DateTime without the time component
    /// </summary>
    public class LastSecondOfDayTimeConverter : IsoDateTimeConverter
    {
        /// <inheritdoc />
        public LastSecondOfDayTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-ddT23:59:59Z";
        }
    }
}
