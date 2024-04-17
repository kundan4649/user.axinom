using System.Security.Claims;

namespace z5.ms.common.validation
{
    /// <inheritdoc/>
    public class UserPrincipal : ClaimsPrincipal
    {
        /// <inheritdoc/>
        public UserPrincipal(AuthToken token)
            : base(new UserIdentity(token))
        {
        }

        /// <inheritdoc/>
        public UserPrincipal(string authenticationMethod)
            : base(new ClaimsIdentity(authenticationMethod))
        {
        }
    }
}
