using System;

namespace z5.ms.common.infrastructure.azure.servicebus
{
    /// <summary>
    /// Credentials for an azure service principal
    /// </summary>
    [Obsolete("Use ManagementClient. Management performed with connection string")]
    public class AzureServicePrincipalOptions
    {
        /// <summary>Tenant Id for the azure service principal used for service bus management operations</summary>
        public string AzureTenantid { get; set; }

        /// <summary>Client Id for the azure service principal used for service bus management operations</summary>
        public string AzureApplicationId { get; set; }

        /// <summary>Client Secret for the azure service principal used for service bus management operations</summary>
        public string AzureClientSecret { get; set; }

        /// <summary>Subscription Id for the azure service principal used for service bus management operations</summary>
        public string AzureSubscriptionId { get; set; }

        /// <summary>Azure resource group name used for service bus management operations</summary>
        public string AzureResourceGroupName { get; set; }
    }
}