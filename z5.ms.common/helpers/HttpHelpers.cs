using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace z5.ms.common.helpers
{
    /// <summary>Helper methods for working with http</summary>
    [Obsolete("Use HttpClientFactory instead: https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests.")]
    public static class HttpHelpers
    {
        static HttpHelpers()
        {
            HttpClient = new HttpClient();
            JsonHttpClient = new HttpClient();
            JsonHttpClient.DefaultRequestHeaders.Accept.Clear();
            JsonHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>Shared http client instance</summary>
        public static HttpClient HttpClient { get; }

        /// <summary>Shared http client instance using json request headers</summary>
        public static HttpClient JsonHttpClient { get; }

        /// <summary>Create a json http response</summary>
        /// <param name="statusCode"></param>
        /// <param name="responseObject"></param>
        /// <returns></returns>
        public static HttpResponseMessage JsonResponse(object responseObject, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            new HttpResponseMessage(statusCode) { Content = new StringContent(JsonConvert.SerializeObject(responseObject), Encoding.UTF8, "application/json") };
    }
}