using Microsoft.AspNetCore.Mvc.Filters;

namespace z5.ms.common.validation.authproviders
{
    /// <summary>Provide authentication and authorization via a JWT token</summary>
    public class JwtAuthProvider : IAuthProvider
    {
        private readonly ITokenValidator _tokenValidator;

        /// <inheritdoc />
        public JwtAuthProvider(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }

        /// <inheritdoc />
        public ValidationResult Authorize(ActionExecutingContext context)
        {
            var result = _tokenValidator.ValidateUser(context);
            if (result.IsValid)
            {
                context.HttpContext.User = new UserPrincipal(result.Token);
            }

            return result;
        }
    }

    /// <summary>Provide authentication and authorization via a JWT token</summary>
    public class JwtAuthAttribute : AuthorizeAttribute
    {
        /// <inheritdoc />
        public JwtAuthAttribute() : base(typeof(JwtAuthProvider))
        {
        }
    }
}