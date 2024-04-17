using System;
using System.Linq;
using System.Reflection;

namespace z5.ms.common.attributes
{
    /// <summary>DTO property title</summary>
    public class TitleAttribute : Attribute
    {
        /// <summary>Title value</summary>
        public string Title { get; set; }

        /// <summary>Default controller</summary>
        public TitleAttribute()
        {
        }

        /// <summary>Default controller</summary>
        /// <param name="title">Title value</param>
        public TitleAttribute(string title)
        {
            Title = title;
        }
    }

    /// <summary>
    /// Title attribute extensions
    /// </summary>
    public static class TitleAttributeExtensions
    {
        /// <summary>Get enum property title</summary>
        /// <param name="enumValue"></param>
        /// <returns>Title string</returns>
        public static string GetTitle(this Enum enumValue)
            => enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TitleAttribute>().Title;
    }
}