using System.Runtime.Serialization;

namespace z5.ms.common.assets
{
    /// <summary>Limits a promotion to a certain group of users</summary>
    public enum TargetAudience
    {
        /// <summary>Default value, promotion is intended for everyone</summary>
        [EnumMember(Value = "all")]
        All,
        
        /// <summary>No previous subscriptions exist for user</summary>
        [EnumMember(Value = "new")]
        New,
        
        /// <summary>User had at least one active subscription during last DroppedUserInactivityMonth (=was subscribed on at least a single day in this period)</summary>
        [EnumMember(Value = "existing")]
        Existing,
        
        /// <summary>User has previous subscription(s), but didn't have any active subscriptions during the last DroppedUserInactivityMonth months (=was not subscribed on any day in this period)</summary>
        [EnumMember(Value = "dropped")]
        Dropped
    }
}