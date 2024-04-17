using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Newtonsoft.Json;
using z5.ms.common.validation;

namespace z5.ms.common.extensions
{
    /// <summary>
    /// Extensions to get information from HttpContext
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Get authenticated type http context 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetCurrentAuthenticationType(this ClaimsPrincipal user)
        {
            return user?.Identity?.AuthenticationType;
        }

        /// <summary>
        /// Get user id from http context 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Guid GetCurrentUserId(this ClaimsPrincipal user)
        {
            if (user is UserPrincipal principal)
                return ((UserIdentity) principal.Identity)?.Token?.UserId ?? Guid.Empty;

            return Guid.TryParse(user?.Claims?
                .FirstOrDefault(c => c.Type == "sub" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?
                .Value, out var guid) ? guid : Guid.Empty;
        }

        /// <summary>
        /// Get system from http context 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetCurrentSystem(this ClaimsPrincipal user)
        {
            if (user is UserPrincipal principal)
                return ((UserIdentity)principal.Identity)?.Token?.System;

            return user?.Claims?.FirstOrDefault(c => c.Type == "system")?.Value;
        }

        /// <summary>
        /// Get system from http context 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetClaim(this ClaimsPrincipal user, string type)
        {
            if (user is UserPrincipal principal)
            {
                var token = ((UserIdentity) principal.Identity)?.Token;
                var property = typeof(AuthToken).GetProperties()
                    .FirstOrDefault(p => p.GetCustomAttributes<JsonPropertyAttribute>()
                        .Any(a => a.PropertyName.Equals(type, StringComparison.OrdinalIgnoreCase)));

                return property?.GetValue(token)?.ToString();
            }

            return user?.Claims?.FirstOrDefault(c => c.Type == type)?.Value;
        }
    }
}
