using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using z5.ms.common.extensions;

namespace z5.ms.common.attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Log all requests to a logger named 'RequestLoggingFilter' in format: {METHOD} {path}{queryString} {body}
    /// <remarks>Optionally response can be added in format: \nResponse status code: {statusCode}\nBody: {repsonsebody} </remarks>
    /// </summary>
    public class RequestLoggingFilterAttribute : ActionFilterAttribute
    {
        /// <summary>Desired level of logs to be created</summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Debug;

        /// <summary>Adds response data into log if true</summary>
        public bool AddResponse { get; set; }

        /// <summary>Adds header data into log if true</summary>
        public bool AddHeaders { get; set; }

        /// <inheritdoc />
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ActionDescriptor.FilterDescriptors
                .FirstOrDefault(a => a.Filter is RequestLogOptionsAttribute)?
                .Filter is RequestLogOptionsAttribute options)
            {
                LogLevel = options.LogLevel;
                AddResponse = options.AddResponse;
                AddHeaders = options.AddHeaders;
            }

            var logger = context.HttpContext.RequestServices.GetService<ILoggerFactory>()
                .CreateLogger("RequestLogging");

            //Request data
            var request = context.HttpContext.Request;
            var log = $"{request.Method.ToUpperInvariant()} {request.Path}{request.GetSafeQueryString()} {request.GetSafeBody()}";
            if (AddHeaders)
                log += $" Headers: {JsonConvert.SerializeObject(request.Headers, Formatting.None)}";

            if (AddResponse)
            {
                //Response data
                try
                {
                    var response = JsonConvert.SerializeObject(context.Result);
                    log += $" Response : {response}";
                    if (AddHeaders)
                        log += $" Headers: {JsonConvert.SerializeObject(request.Headers, Formatting.None)}";
                }
                catch
                {
                    log += " Response body couldn't be parsed";
                }
            }

            LogExtensions.Log(logger, LogLevel, log);

            base.OnActionExecuted(context);
        }
    }

    /// <summary>
    /// Request log options attribute to override logging settings for some controllers 
    /// </summary>
    public class RequestLogOptionsAttribute : ActionFilterAttribute
    {
        /// <summary>Desired level of logs to be created</summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Debug;

        /// <summary>Adds response data into log if true</summary>
        public bool AddResponse { get; set; } = false;

        /// <summary>Adds headers data into log if true</summary>
        public bool AddHeaders { get; set; } = false;
    }
}
