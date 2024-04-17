using Newtonsoft.Json;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>Base command definition to resend confirmation notification</summary>
    public class ResendConfirmationCommand
    {
        /// <summary>Type of the authentication method</summary>
        [JsonIgnore]
        public virtual AuthenticationMethod Type { get; set; }

        /// <summary>Version of the endpoint</summary>
        [JsonIgnore]
        public int Version { get; set; }
    }
}