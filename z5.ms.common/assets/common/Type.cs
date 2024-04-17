using System;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace z5.ms.common.assets.common
{
    /// <summary>The item type to search</summary>
    [GeneratedCode("NJsonSchema", "9.4.2.0")]
    [Obsolete("not used", true)]
    public enum Type
    {
        /// <summary>A movie</summary>
        [EnumMember(Value = "Movie")] Movie = 0,

        /// <summary>A video. This includes movies and trailers</summary>
        [EnumMember(Value = "Video")] Video = 1,

        /// <summary>Music</summary>
        [EnumMember(Value = "Music")] Music = 2,

        /// <summary>Original</summary>
        [EnumMember(Value = "Original")] Original = 3,

        /// <summary>Tv show</summary>
        [EnumMember(Value = "TvShow")] TvShow = 4,

        /// <summary>Channel</summary>
        [EnumMember(Value = "Channel")] Channel = 5
    }
}