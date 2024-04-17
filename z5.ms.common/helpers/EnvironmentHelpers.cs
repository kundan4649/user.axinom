using System;

namespace z5.ms.common.helpers
{
    /// <summary>
    /// Helper functions for environment variables and properties
    /// </summary>
    public static class EnvironmentHelpers
    {
        /// <summary>
        /// Env variable for disabling startup of API services. Should be used when the API is run to only generate Swagger doc
        /// </summary>
        private const string DisableServiceInit = "DISABLE_SERVICE_INIT";
        
        /// <summary>
        /// Check variable for disabling API services is set 
        /// </summary>
        public static bool ServiceInitDisabled() 
            => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(DisableServiceInit));    
    }
}