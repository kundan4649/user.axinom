using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>Remind for upcoming EPG program via</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ReminderType
    {
        /// <summary>Email</summary>
        [EnumMember(Value = "Email")]
        Email = 0,
 
        /// <summary>Mobile</summary>
        [EnumMember(Value = "Mobile")]
        Mobile = 1,

    }
}
