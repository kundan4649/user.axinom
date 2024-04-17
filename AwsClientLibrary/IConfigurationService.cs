using Microsoft.Extensions.Configuration;

namespace AwsClientLibrary
{
    public interface IConfigurationService
    {
        IConfiguration GetConfiguration();
    }
}