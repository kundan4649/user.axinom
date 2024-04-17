using System.Runtime.Serialization;

namespace z5.ms.common.assets
{
    /// <summary> Target enumeration for external link asset </summary>
    public enum ExternalLinkTarget
    {
        /// <summary> WebView </summary>
        [EnumMember(Value = "web_view")]
        WebView,

        /// <summary> InternalLink </summary>
        [EnumMember(Value = "internal_link")]
        InternalLink,

        /// <summary> ExternalLink </summary>
        [EnumMember(Value = "external_link")]
        ExternalLink,

        /// <summary> WebViewUserInfo </summary>
        [EnumMember(Value = "webview_userinfo")]
        WebViewUserInfo,

        /// <summary> SdkUserInfo </summary>
        [EnumMember(Value = "sdk_userinfo")]
        SdkUserInfo
    }
}
