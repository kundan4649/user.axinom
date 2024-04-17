using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace z5.ms.common.validation.obsolete
{
    /// <inheritdoc/>
    [Obsolete("Use AuthorizeAttribute with BasicAuthProvider")]
    public abstract class BasicAuthAttribute : ActionFilterAttribute
    {
        private readonly string _usernameConfigEntryName;
        private readonly string _passwordConfigEntryName;
        
        /// <inheritdoc />
        protected BasicAuthAttribute(string usernameConfigEntryName, string passwordConfigEntryName)
        {
            _usernameConfigEntryName = usernameConfigEntryName;
            _passwordConfigEntryName = passwordConfigEntryName;
        }
        
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var tokenValidator =
                (ITokenValidator)context.HttpContext.RequestServices.GetService(typeof(ITokenValidator));

            var result = tokenValidator.ValidateUserBasic(context, "basic", _usernameConfigEntryName, _passwordConfigEntryName);

            if (!result.IsValid)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult(result.Error);
            }

            base.OnActionExecuting(context);
        }
    }
}