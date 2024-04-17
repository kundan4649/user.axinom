using Newtonsoft.Json;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>Base command definition to send random code to user that can be used for change forgotten password</summary>
    public class ForgotPasswordCommand
    {
        /// <summary>Type of the authentication method</summary>
        [JsonIgnore]
        public virtual AuthenticationMethod Type { get; set; }

        /// <summary>Version of the endpoint</summary>
        [JsonIgnore]
        public int Version { get; set; }
    }
}