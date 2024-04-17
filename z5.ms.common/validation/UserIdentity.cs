using System;
using System.Security.Claims;

namespace z5.ms.common.validation
{
    /// <inheritdoc/>
    public class UserIdentity : ClaimsIdentity
    {
        /// <summary>User Token</summary>
        public AuthToken Token { get; set; }

        /// <inheritdoc/>
        public override string AuthenticationType => "user";

        /// <inheritdoc/>
        public UserIdentity()
        {
            Token = new AuthToken { UserId = new Guid(), UserEmail = "", UserMobile = "" };
        }

        /// <inheritdoc/>
        public UserIdentity(AuthToken token)
        {
            Token = token;
        }
    }
}
