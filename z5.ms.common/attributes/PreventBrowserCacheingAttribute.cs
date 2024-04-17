using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace z5.ms.common.attributes
{
    /// <summary>Adds response headers to prevent browser caching</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PreventBrowserCacheingAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuted(ActionExecutedContext actionContext)
        {
            actionContext.HttpContext.Response.Headers.Add("Cache-Control", "no-cache, no-store");
            actionContext.HttpContext.Response.Headers.Add("Expires", "-1");
        }
    }
}