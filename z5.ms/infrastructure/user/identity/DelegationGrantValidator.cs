using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace z5.ms.infrastructure.user.identity
{
    /// <inheritdoc />
    /// <summary>Handles validation of token requests for delegation grant</summary>
    public class DelegationGrantValidator : IExtensionGrantValidator
    {
        /// <inheritdoc />
        public DelegationGrantValidator()
        {
        }

        /// <inheritdoc />
        public string GrantType => "delegation";

        /// <inheritdoc />
        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            await Task.CompletedTask;
            var sub = context.Request.Raw.Get("sub");

            if (string.IsNullOrEmpty(sub))
            {
                // TODO: log error
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
                return;
            }

            var country = context.Request.Raw.Get("current_country");
            var claims = country == null
                ? null : new[] {new Claim(Z5Claims.CurrentCountry, country)};

            context.Result = new GrantValidationResult(sub, GrantType, claims);
        }
    }
}