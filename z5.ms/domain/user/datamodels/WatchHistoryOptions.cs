namespace z5.ms.domain.user.datamodels
{
    /// <summary> Options for watch history feature </summary>
    public class WatchHistoryOptions
    {
        public bool AsyncAdd { get; set; }
        public bool AsyncDelete { get; set; }
        public bool AsyncUpdate { get; set; }
    }
}
