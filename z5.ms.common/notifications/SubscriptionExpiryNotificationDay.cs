namespace z5.ms.common.notifications
{
    /// <summary>Shows how far is the subscription from expiring. Used to send notification messages.</summary>
    public enum SubscriptionExpiryNotificationDay
    {
        FiveDaysAgo,
        TwoDaysAgo,
        OneDayAgo,
        Today,
        InOneDay,
        InTwoDays
    }
}