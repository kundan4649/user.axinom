using Microsoft.AspNetCore.Mvc.Filters;

namespace z5.ms.common.validation.authproviders
{
    /// <summary>Authentication via a secret defined in configuration file and a signed request</summary>
    public abstract class SecretSignedAuthProvider : IAuthProvider
    {
        private readonly string _scheme;
        private readonly string _secretConfigEntryName;
        private readonly string _signingKeyConfigEntry;

        /// <inheritdoc />
        protected SecretSignedAuthProvider(string scheme, string secretConfigEntryName, string signingKeyConfigEntry)
        {
            _scheme = scheme;
            _secretConfigEntryName = secretConfigEntryName;
            _signingKeyConfigEntry = signingKeyConfigEntry;
        }

        /// <inheritdoc />
        protected SecretSignedAuthProvider(string secretConfigEntryName, string signingKeyConfigEntry)
        {
            _secretConfigEntryName = secretConfigEntryName;
            _signingKeyConfigEntry = signingKeyConfigEntry;
        }

        /// <inheritdoc />
        public ValidationResult Authorize(ActionExecutingContext context)
        {
            var tokenValidator =
                (ITokenValidator)context.HttpContext.RequestServices.GetService(typeof(ITokenValidator));

            return tokenValidator.ValidateSecretAndSignature(context, _scheme, _secretConfigEntryName, _signingKeyConfigEntry);
        }
    }
}