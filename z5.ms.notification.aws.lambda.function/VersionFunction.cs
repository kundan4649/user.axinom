using Amazon.Lambda.Core;
using AWS.Lambda.Configuration.Common;
using System.Net.Http;
using System.Reflection;

namespace z5.ms.notification.aws.lambda.function
{
    public class VersionFunction : FunctionAbstract
    {
        public HttpResponseMessage Version(ILambdaContext context)
        {
            var versionInfo = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            return AWSHelper.JsonResponse(versionInfo);
        }
    }
}