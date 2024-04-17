using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace z5.ms.common.abstractions
{
    /// <summary>
    /// Sets the response HTTP status code and adds the error object as json to the response body
    /// </summary>
    public class JsonErrorResult : JsonResult
    {
        // TODO: ErrorFields

        /// <inheritdoc />
        public JsonErrorResult(int errorCode, string message, HttpStatusCode httpStatus = HttpStatusCode.BadRequest) : base(new Error { Code = errorCode, Message = message })
        {
            StatusCode = (int)httpStatus;
        }

        /// <inheritdoc />
        public JsonErrorResult(Error error, HttpStatusCode httpStatus = HttpStatusCode.BadRequest) : base(error)
        {
            StatusCode = (int)httpStatus;
        }

        /// <inheritdoc />
        public JsonErrorResult(IErrorResult result) : base(result.Error)
        {
            StatusCode = result.StatusCode;
        }
    }
}
