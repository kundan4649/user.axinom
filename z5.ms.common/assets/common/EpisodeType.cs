using System.Runtime.Serialization;

namespace z5.ms.common.assets.common
{
    /// <summary>The available episode types</summary>
    public enum EpisodeType
    {
        /// <summary>Regular episode</summary>
        [EnumMember(Value = "episode")] Episode = 0,

        /// <summary>Web TV episode</summary>
        [EnumMember(Value = "webisode")] Webisode = 1,

        /// <summary>Episode preview</summary>
        [EnumMember(Value = "preview")] Preview = 2
    }
}