using z5.ms.domain.user.datamodels;

namespace z5.ms.domain.user.messages
{
    /// <summary> Queue message definition for watch history operations </summary>
    public class WatchHistoryMessage
    {
        /// <summary>Type of the message add/update/delete</summary>
        public MessageType Type { get; set; }

        /// <summary>Watch history item to be added/updated/deleted</summary>
        public WatchHistoryEntity Item { get; set; }
    }
}
