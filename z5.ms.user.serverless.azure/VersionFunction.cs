using System.Net.Http;
using System.Reflection;
using z5.ms.common.infrastructure.azure.serverless;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace z5.ms.user.serverless.azure
{
    public static class VersionFunction
    {
        /// <summary>Get version function returns assembly version</summary>
        [FunctionName("Version")]
        public static HttpResponseMessage Version(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "version")]HttpRequestMessage request,
            ILogger rawlog)
        {
            var versionInfo = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
            return AzureHelpers.JsonResponse(versionInfo);
        }
    }
}