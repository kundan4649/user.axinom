using System;
using System.Collections.Generic;
using Microsoft.Extensions.HealthChecks;

namespace z5.ms.common.healthcheck
{
    /// <summary>
    /// Extensions for health checks
    /// </summary>
    public static class HealthCheckHelpers
    {
        private static readonly Dictionary<string, string> CheckDescriptions = new Dictionary<string, string>();

        /// <summary>
        /// Extension to get check description
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public static string GetDescription(this CachedHealthCheck check)
        {
            return CheckDescriptions[check.Name];
        }

        /// <summary>
        /// Add a new check to health check list
        /// </summary>
        /// <param name="checks"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="check"></param>
        /// <param name="cacheDurationSec"></param>
        public static void AddCustomCheck(this HealthCheckBuilder checks, string name, string description,
            IHealthCheck check, int cacheDurationSec = 1)
        {
            checks.AddCheck(name, check, TimeSpan.FromSeconds(cacheDurationSec));
            CheckDescriptions.Add(name, description);
        }
    }
}
