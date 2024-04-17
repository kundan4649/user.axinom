using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace z5.ms.common.validation.obsolete
{
    /// <summary>Authentication via a secret defined in configuration file</summary>
    [Obsolete("Use AuthorizeAttribute with SecretAuthProvider")]
    public abstract class SecretAuthAttribute : ActionFilterAttribute
    {
        private readonly string _scheme;
        private readonly string _configEntryName;
        private readonly string _scheme2;
        private readonly string _configEntryName2;

        /// <summary> Signing key configuration value </summary>
        public string SigningKeyConfigEntry { get; set; }

        /// <inheritdoc />
        protected SecretAuthAttribute(string scheme, string configEntryName, string scheme2 = null, string configEntryName2 = null)
        {
            _scheme = scheme;
            _configEntryName = configEntryName;
            _scheme2 = scheme2;
            _configEntryName2 = configEntryName2;
        }

        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var tokenValidator =
                (ITokenValidator)context.HttpContext.RequestServices.GetService(typeof(ITokenValidator));

            var result = ValidateSecret(tokenValidator, _scheme, _configEntryName, context);
            if (result.IsValid)
                context.HttpContext.User = new UserPrincipal(_scheme);

            if (!result.IsValid && _configEntryName2 != null)
            {
                result = tokenValidator.ValidateSecret(context, _scheme2, _configEntryName2);
                if (result.IsValid)
                    context.HttpContext.User = new UserPrincipal(_scheme2);
            }

            if (!result.IsValid)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult(result.Error);
            }

            base.OnActionExecuting(context);
        }

        private ValidationResult ValidateSecret(ITokenValidator tokenValidator, string scheme, string configEntryName, ActionExecutingContext context)
        {
            return SigningKeyConfigEntry == null
                ? tokenValidator.ValidateSecret(context, scheme, configEntryName)
                : tokenValidator.ValidateSecretAndSignature(context, scheme, configEntryName, SigningKeyConfigEntry);
        }
    }
}