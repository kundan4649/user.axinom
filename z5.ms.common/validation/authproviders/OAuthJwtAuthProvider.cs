using Microsoft.AspNetCore.Mvc.Filters;
using z5.ms.common.extensions;

namespace z5.ms.common.validation.authproviders
{
    /// <summary>Provide authentication and authorization via a JWT token</summary>
    public class OAuthJwtAuthProvider : IAuthProvider
    {
        private readonly JwtTokenValidator _tokenValidator;

        /// <inheritdoc />
        public OAuthJwtAuthProvider(JwtTokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }

        /// <inheritdoc />
        public ValidationResult Authorize(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.GetBearerToken();

            var validationResult = _tokenValidator.ValidateJwtToken(token);
            if (validationResult.IsValid)
                context.HttpContext.User = validationResult.ClaimsPrincipal;
            return validationResult;
        }
    }
}
