namespace z5.ms.common.infrastructure.events
{
    /// <summary>Settings for sync and assets cache functionality</summary>
    public class EventBusOptions
    {
        /// <summary>Service bus connection for cross domain events</summary>
        public string MsEventBusConnection { get; set; }

        /// <summary>Service bus name for cross domain events</summary>
        public string MsEventBusName { get; set; }

        /// <summary>The name of this service. This property is used to identify service bus subscriptions relating to this service</summary>
        public string MsServiceName { get; set; }
    }
}
