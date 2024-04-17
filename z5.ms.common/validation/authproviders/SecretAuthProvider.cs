using Microsoft.AspNetCore.Mvc.Filters;

namespace z5.ms.common.validation.authproviders
{
    /// <summary>Authentication via a secret defined in configuration file</summary>
    public abstract class SecretAuthProvider : IAuthProvider
    {
        private readonly string _scheme;
        private readonly string _configEntryName;

        /// <inheritdoc />
        protected SecretAuthProvider(string scheme, string configEntryName)
        {
            _scheme = scheme;
            _configEntryName = configEntryName;
        }
        
        /// <inheritdoc />
        protected SecretAuthProvider(string configEntryName)
        {
            _configEntryName = configEntryName;
        }

        /// <inheritdoc />
        public ValidationResult Authorize(ActionExecutingContext context)
        {
            var tokenValidator =
                (ITokenValidator) context.HttpContext.RequestServices.GetService(typeof(ITokenValidator));

            var result = tokenValidator.ValidateSecret(context, _scheme, _configEntryName);
            if (result.IsValid)
                context.HttpContext.User = new UserPrincipal(_scheme);

            return result;
        }
    }
}