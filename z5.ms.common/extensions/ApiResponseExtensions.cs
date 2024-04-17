using Microsoft.AspNetCore.Mvc;
using z5.ms.common.abstractions;

namespace z5.ms.common.extensions
{
    /// <summary>
    /// API response extensions
    /// </summary>
    public static class ApiResponseExtensions
    {
        /// <summary>Return API error response model with a single error and selected HTTP status code</summary>
        /// <param name="controller">API controller object</param>
        /// <param name="httpStatusCode">Supported HTTP status code</param>
        /// <param name="errorCode">A value from error codes enumeration</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns></returns>
        public static JsonResult Error(this Controller controller, int httpStatusCode, int errorCode, string errorMessage)
        {
            if (controller.HttpContext != null) //unit tests
                controller.HttpContext.Response.StatusCode = httpStatusCode;

            return new JsonResult(new Error { Code = errorCode, Message = errorMessage });
        }

        /// <summary>Return API error response model with a single error and selected HTTP status code</summary>
        /// <param name="controller">API controller object</param>
        /// <param name="httpStatusCode">Supported HTTP status code</param>
        /// <param name="error">Validation error</param>
        /// <returns></returns>
        public static JsonResult Error(this Controller controller, int httpStatusCode, Error error)
        {
            if (controller.HttpContext != null) //unit tests
                controller.HttpContext.Response.StatusCode = httpStatusCode;

            return new JsonResult(error);
        }
    }
}