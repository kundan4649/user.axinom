using System.Net;
using Microsoft.AspNetCore.Mvc;
using z5.ms.common.abstractions;

namespace z5.ms.common.extensions
{
    /// <summary>Result extensions</summary>
    public static class ResultExtensions
    {
        /// <summary>Convert a result to a json result to be returned from a controller</summary>
        public static JsonResult ToJsonResult<T>(this Result<T> result) 
            => result.Success ? new JsonResult(result.Value) : new JsonErrorResult(result.Error, (HttpStatusCode)result.StatusCode);
    }
}