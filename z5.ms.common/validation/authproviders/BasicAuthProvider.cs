using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace z5.ms.common.validation.authproviders
{
    /// <summary>Basic http authentication using a username and password defined in configuration</summary>
    public abstract class BasicAuthProvider : IAuthProvider
    {
        private readonly string _usernameConfigEntryName;
        private readonly string _passwordConfigEntryName;

        /// <inheritdoc />
        protected BasicAuthProvider(string usernameConfigEntryName, string passwordConfigEntryName)
        {
            _usernameConfigEntryName = usernameConfigEntryName;
            _passwordConfigEntryName = passwordConfigEntryName;
        }

        /// <inheritdoc />
        public ValidationResult Authorize(ActionExecutingContext context)
        {
            var tokenValidator =
                (ITokenValidator)context.HttpContext.RequestServices.GetService(typeof(ITokenValidator));

            var result = tokenValidator.ValidateUserBasic(context, "basic", _usernameConfigEntryName, _passwordConfigEntryName);

            if (!result.IsValid)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult(result.Error);
            }

            return result;
        }
    }
}