using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace z5.ms.common.extensions
{
    /// <summary>
    /// Extensions to get information from HttpResponse
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Get request body as string
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string GetBody(this HttpResponse response)
        {
            try
            {
                if (!response.Body.CanSeek)
                    return "";

                using (var stream = response.Body)
                {
                    stream.Position = 0;
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                return $"Can't read body - {e.Message}";
            }
        }
    }
}
