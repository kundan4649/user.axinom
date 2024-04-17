using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using z5.ms.common.abstractions;

namespace z5.ms.common.validation
{
    /// <summary>Provider authorization for controller actions</summary>
    public interface IAuthProvider
    {
        /// <summary>Authorize an action context</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        ValidationResult Authorize(ActionExecutingContext context);
    }

    /// <summary>Authorization based on one or more IAuthorizationProviders</summary>
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>Read only authentication providers. Enables registering different authentication per endpoint.</summary>
        public readonly List<Type> AuthProviders;

        /// <summary>Construct from one or more auth provider types. Each type must implement IAuthProvider</summary>
        /// <param name="authTypes"></param>
        public AuthorizeAttribute(params Type[] authTypes)
        {
            AuthProviders = authTypes.Select(type =>
            {
                if (!typeof(IAuthProvider).IsAssignableFrom(type))
                    throw new Exception($"typeof({type.Name}) is not a valid parameter for {nameof(AuthorizeAttribute)}");
                return type;
            }).ToList();
        }

        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ValidationResult result = null;
            var error = new Error { Code = 401, Message = "Authorization failed", Fields = new List<ErrorField>() };
            foreach (var type in AuthProviders)
            {
                if (!((context.HttpContext.RequestServices.GetService(type)
                      ?? Activator.CreateInstance(type)) is IAuthProvider provider))
                    continue;

                result = provider.Authorize(context);
                if (result.IsValid)
                    break;
                error.Fields.Add(new ErrorField { Field = type.Name, Message = result.Error.Message });
            }
            if (result == null || !result.IsValid)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult(error);
            }
            base.OnActionExecuting(context);
        }
    }
}