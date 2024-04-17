using System.Runtime.Serialization;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>Enum definition for available authentication methods of a user</summary>
    public enum AuthenticationMethod
    {
        [EnumMember(Value = "Email")]
        Email,

        [EnumMember(Value = "Mobile")]
        Mobile,

        [EnumMember(Value = "FacebookUserId")]
        Facebook,

        [EnumMember(Value = "GoogleUserId")]
        Google,

        [EnumMember(Value = "GoogleUserId")]
        GoogleWithTokenId,

        [EnumMember(Value = "TwitterUserId")]
        Twitter,

        [EnumMember(Value = "AmazonUserId")]
        Amazon
    }
}