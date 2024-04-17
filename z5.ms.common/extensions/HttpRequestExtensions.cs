using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace z5.ms.common.extensions
{
    /// <summary>
    /// Extensions to get information from HttpRequest
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Get the remote ip address for a request.
        /// </summary>
        /// <param name="request">The relevant HttpRequest.</param>
        public static string GetRemoteIp(this HttpRequest request)
        {
            if (request.TryGetHeader("True-Client-IP", out var trueClientIp))
                return trueClientIp;
            if (request.TryGetHeader("X-Forwarded-For", out var xForwardedForIp))
                return xForwardedForIp;
            return request?.HttpContext.Connection.RemoteIpAddress.ToString();
        }
        
        /// <summary>Get a field from the request headers by key.</summary>
        /// <param name="request">The relevant HttpRequest.</param>
        /// <param name="header">The header key to get.</param>
        /// <param name="result">The key that was found from the headers. Null or empty if it could not be found.</param>
        /// <returns>True if the result is not empty or null. False otherwise.</returns>
        public static bool TryGetHeader(this HttpRequest request, string header, out string result)
        {
            result = request?.Headers[header].FirstOrDefault();
            return !string.IsNullOrWhiteSpace(result);
        }
        
        
        /// <summary>
        /// Sends a log if response time exceeds the specified timeout  
        /// </summary>
        /// <remarks>Sends warning log as default if log level is not specified</remarks>
        /// <param name="task"></param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <param name="logger">Logger to send logs</param>
        /// <param name="message">Log message</param>
        /// <param name="level">Desired log level</param>
        /// <returns></returns>
        public static Task<T> LogIfTimeout<T>(this Task<T> task, TimeSpan timeout, ILogger logger, string message, LogLevel level = LogLevel.Warning)
        {
            Task.Delay(timeout).ContinueWith(a =>
            {
                if (!task.IsCompleted)
                    logger?.Log(level, message);
            });
            return task;
        }

        /// <summary>
        /// Throws Timeout Exception for the specified timeout  
        /// </summary>
        /// <param name="task"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout)
        {
            return Task.WhenAny(task, Task.Delay(timeout)).ContinueWith(a =>
            {
                if (!task.IsCompleted)
                    throw new TimeoutException("Timeout exceeded");
                return task;
            }).Result;
        }

        /// <summary>
        /// Get query string without password parameters for logging purposes
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetSafeQueryString(this HttpRequest request)
        {
            try
            {
                if (!request.Query.Any())
                    return "";

                var query = request.Query.ToDictionary(a => a.Key, a => a.Value);

                var passwords = query.Where(a => a.Key.IndexOf("password", StringComparison.OrdinalIgnoreCase) > -1).ToList();
                foreach (var item in passwords)
                {
                    query[item.Key] = "xxxxxx";
                }

                var queryString = string.Join("&", query.Select(a => $"{a.Key}={a.Value}").ToArray());

                return string.IsNullOrWhiteSpace(queryString) ? "" : $"?{queryString}";
            }
            catch (Exception e)
            {
                return $"Can't read query string - {e.Message}";
            }
        }

        /// <summary>
        /// Get request body as string
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetBody(this HttpRequest request)
        {
            using (var bodyStream = new MemoryStream())
            {
                try
                {
                    request.EnableRewind();
                    request.Body.Position = 0;
                    request.Body.CopyTo(bodyStream);
                    if (!request.Body.CanSeek)
                        return "";

                    bodyStream.Position = 0;
                    using (var reader = new StreamReader(bodyStream, Encoding.UTF8))
                    {
                        var body = reader.ReadToEnd();

                        if (string.IsNullOrWhiteSpace(body) && request.HasFormContentType)
                            return JsonConvert.SerializeObject(request.Form.ToDictionary(a => a.Key, a => a.Value));

                        return body;
                    }
                }
                catch (Exception e)
                {
                    return $"Can't read body - {e.Message}";
                }
            }
        }

        /// <summary>
        /// Get request body as string without password parameters for logging purposes
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetSafeBody(this HttpRequest request)
        {
            using (var bodyStream = new MemoryStream())
            {
                try
                {
                    request.EnableRewind();
                    request.Body.Position = 0;
                    request.Body.CopyTo(bodyStream);

                    if (!request.Body.CanSeek)
                        return "";

                    bodyStream.Position = 0;
                    using (var reader = new StreamReader(bodyStream, Encoding.UTF8))
                    {
                        var body = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(body) && request.HasFormContentType)
                            return JsonConvert.SerializeObject(request.Form.ToDictionary(a => a.Key, a => a.Value));

                        try
                        {
                            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
                            if (!dictionary.Any())
                                return body;

                            var passwords = dictionary.Where(a => a.Key.IndexOf("password", StringComparison.OrdinalIgnoreCase) > -1).ToList();
                            foreach (var item in passwords)
                            {
                                dictionary[item.Key] = "xxxxxx";
                            }

                            return JsonConvert.SerializeObject(dictionary);
                        }
                        catch
                        {
                            return body;
                        }
                    }
                }
                catch (Exception e)
                {
                    return $"Can't read body - {e.Message}";
                }
            }
        }

        /// <summary>
        /// Extension to get bearer token from Authorization header
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetBearerToken(this HttpRequest request)
        {
            var token = request.Headers
                .FirstOrDefault(a => a.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase)).Value
                .ToString().Split(' ');
            if (token.Length != 2 || !token[0].Equals("bearer", StringComparison.OrdinalIgnoreCase))
                return null;

            return token[1];
        }
    }
}