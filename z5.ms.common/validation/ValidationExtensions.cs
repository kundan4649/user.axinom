using Microsoft.AspNetCore.Http;

namespace z5.ms.common.validation
{
    /// <summary> User validation extensions </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Get authenticated user id from http context 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static UserIdentity GetCurrentIdentity(this HttpContext context)
        {
            var user = (UserPrincipal)context?.User;
            return user == null ? new UserIdentity() : user.Identity as UserIdentity;
        }
    }
}
