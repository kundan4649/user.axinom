using Microsoft.AspNetCore.Mvc.Filters;

namespace z5.ms.common.validation.authproviders
{
    /// <summary>Allows anonymous access if authorization header is not supplied</summary>
    public class AnonymousAuthProvider : IAuthProvider
    {
        /// <inheritdoc />
        public ValidationResult Authorize(ActionExecutingContext context)
        {
            return context.HttpContext.Request.Headers.ContainsKey("Authorization")
                ? new ValidationResult("Invalid authentication token")
                : new ValidationResult {IsValid = true};
        }
    }
}