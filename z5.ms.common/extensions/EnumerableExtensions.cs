using System.Collections.Generic;
using System.Linq;

namespace z5.ms.common.extensions
{
    /// <summary>Extension methods for enumerables</summary>
    public static class EnumerableExtensions
    {
        /// <summary>True, if collection is null or empty</summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
            => collection == null || !collection.Any();

        /// <summary>True if collection is not null and contains given item</summary>s
        public static bool IsNotNullAndContains<T>(this IEnumerable<T> collection, T item) =>
            collection != null && collection.Contains(item);
    }
}
