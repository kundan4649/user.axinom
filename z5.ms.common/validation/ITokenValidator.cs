using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace z5.ms.common.validation
{
    /// <summary>
    /// Token validation service interface
    /// </summary>
    public interface ITokenValidator
    {
        /// <summary>
        /// Validate user authentication token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        TokenValidationResult ValidateUser(ActionExecutingContext context);

        /// <summary>
        /// Validate Secret authentication token
        /// </summary>
        /// <remarks>If scheme is null then the secret config entry mist include scheme</remarks>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="configEntryName"></param>
        /// <returns></returns>
        ValidationResult ValidateSecret(ActionExecutingContext context, string scheme, string configEntryName);

        /// <summary>
        /// Validate Secret authentication token combined with a request signature
        /// </summary>
        /// <remarks>If scheme is null then the secret config entry mist include scheme</remarks>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="secretConfigEntryName"></param>
        /// <param name="signingKeyConfigEntryName"></param>
        /// <returns></returns>
        ValidationResult ValidateSecretAndSignature(ActionExecutingContext context, string scheme, string secretConfigEntryName, string signingKeyConfigEntryName);

        /// <summary>
        /// Validate user authentication token string
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        TokenValidationResult ValidateUserToken(string tokenString);

        /// <summary>
        /// Validate a user with a fixed username and password from app configuration
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="usernameConfigEntryName"></param>
        /// <param name="passwordConfigEntryName"></param>
        /// <returns></returns>
        ValidationResult ValidateUserBasic(ActionExecutingContext context, string scheme, string usernameConfigEntryName, string passwordConfigEntryName);

        /// <summary>
        /// Validate user authentication token string and return user id
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        Guid GetUserId(string tokenString);
    }
}
