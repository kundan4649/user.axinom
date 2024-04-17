using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>The gender of the user</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Gender
    {
        /// <summary>Unknown</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>Male</summary>
        [EnumMember(Value = "male")]
        Male = 1,

        /// <summary>Female</summary>
        [EnumMember(Value = "female")]
        Female = 2, 
        
        /// <summary>Other</summary>
        [EnumMember(Value = "other")]
        Other = 3,
    }
}
