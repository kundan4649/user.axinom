using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using z5.ms.common.validation.authproviders;

namespace z5.ms.common.validation.obsolete
{
    /// <inheritdoc />
    [Obsolete("Use JwtAuthAttribute")]
    public class AuthAttribute : JwtAuthAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization") && IsAnonymousAllowed(context))
            {
                context.HttpContext.User = null;
                base.OnActionExecuting(context);
                return;
            }

            base.OnActionExecuting(context);
        }

        private static bool IsAnonymousAllowed(ActionContext context)
        {
            var actionDecriptor =
                context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
            var methodAttributes = actionDecriptor?.MethodInfo?.GetCustomAttributes(true);
            var allowAnonymous = methodAttributes?.Any(a => a is AllowAnonymousAttribute);
            return allowAnonymous ?? false;
        }
    }
}