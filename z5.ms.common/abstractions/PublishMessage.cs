using System.Diagnostics.CodeAnalysis;

namespace z5.ms.common.abstractions
{
    /// <summary>CMS publish message</summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class PublishMessage
    {
        /// <summary>Publication message operation type</summary>
        public PublishMessageOperationType Type { get; set; }
        
        /// <summary>Published asset ID</summary>
        public string AssetId { get; set; }
        
        /// <summary>Publication ID</summary>
        public string PublishId { get; set; }
    }

    /// <summary>Publication message operation type</summary>
    public enum PublishMessageOperationType
    {
        /// <summary>Use operation type provided by parent object</summary>
        Default,

        /// <summary>Add file(s), without removing any content</summary>
        Append,

        /// <summary>Remove previously existing file(s) before adding new one</summary>
        Replace,

        /// <summary>Removes all unmentioned files on upon completion</summary>
        PostCompare
    }
}