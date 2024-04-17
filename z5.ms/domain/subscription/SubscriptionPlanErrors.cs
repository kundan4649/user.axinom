using z5.ms.common.abstractions;

namespace z5.ms.domain.subscription
{
    //TODO define actual error codes instead of '1'
    //TODO discuss if we should merge method/context specific errors together
    /// <summary>Errors used by Subscription Module </summary>
    public class SubscriptionPlanErrors
    {
        /// <summary>Subscription plan couldn't be found </summary>
        public Error PlanNotFound { get; } = new Error { Code = 2, Message = "Subscription plan not found" };
    }
}