using System;

namespace AWS.Lambda.Configuration.Common
{
    public class EnvironmentService : IEnvironmentService
    {
        public EnvironmentService()
        {
            EnvironmentName = Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.AspnetCoreEnvironment)
                              ?? Constants.Environments.Production;
        }

        public string EnvironmentName { get; set; }
    }
}
