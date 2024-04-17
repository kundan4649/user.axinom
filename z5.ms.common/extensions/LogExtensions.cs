using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Newtonsoft.Json;

namespace z5.ms.common.extensions
{
    /// <summary>
    /// Extensions for logging
    /// </summary>
    public static class LogExtensions
    {
        /// <summary>
        /// Logging with desired log level
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="args"></param>
        public static void Log(this ILogger logger, LogLevel level, string message, params object[] args)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(level, 0, new FormattedLogValues(message, args), null, (state, ex) => state.ToString());
        }

        /// <summary>
        /// Logging full request data with exception
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="request"></param>
        /// <param name="ex"></param>
        public static void LogRequestWithException(this ILogger logger, HttpRequest request, Exception ex)
        {
            try
            {
                var log = $"Request : {request.Method.ToUpperInvariant()} {request.Path}{request.GetSafeQueryString()} " +
                          (request.ContentLength > 0 ? $"Body : {request.GetSafeBody()} " : "") +
                          $"Headers : {JsonConvert.SerializeObject(request.Headers)}";
                logger?.LogError(ex, log);
            }
            catch (Exception e)
            {
                logger?.LogError(e, "Couldn't get request data");
            }
        }

        /// <summary>
        /// Sends a log to show duration of the task
        /// </summary>
        /// <remarks>Sends warning log as default if log level is not specified</remarks>
        /// <param name="task"></param>
        /// <param name="logger">Logger to send logs</param>
        /// <param name="taskIdentifier">Some text to identify the running task in the logs</param>
        /// <param name="level">Desired log level</param>
        /// <returns></returns>
        public static Task<T> LogDuration<T>(this Task<T> task, ILogger logger, string taskIdentifier = null, LogLevel level = LogLevel.Information)
        {
            var sw = new Stopwatch();
            sw.Start();
            task.ContinueWith(_ =>
                {
                    sw.Stop();
                    logger?.Log(level, $"It took {sw.ElapsedMilliseconds} milliseconds to complete the task. / {taskIdentifier}");
                }
            );

            return task;
        }
    }
}
