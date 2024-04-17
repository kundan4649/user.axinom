using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <summary>Collection filtering and validation extensions</summary>
    public static class CacheQueryExtensions
    {
        /// <summary>Validate that optional field belongs to a specified collection</summary>
        /// <remarks>if items is null, returns true (optional query field)</remarks>
        public static bool IsInCollectionOrNull(this string item, params string[] items)
            => string.IsNullOrEmpty(item)
               || items.Any(i => item.Equals(i, StringComparison.OrdinalIgnoreCase));

        /// <summary>True, if items from collection 1 intersect with items from collection 2</summary>
        /// <remarks>Example: asset.Genres.Intersects(message.Genres) - asset has genres that were specified in a parameters list</remarks>
        public static bool Intersects(this IEnumerable<string> list1, IEnumerable<string> list2, bool ignoreCase = true)
        {
            if (list1 == null || list2 == null)
                return false;

            return list1.Intersect(list2, ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal).Any();
        }

        /// <summary>
        /// Finds an ordering expression for specified field name. First property that matches field name is used,
        /// `_` and Case are ignored.
        /// </summary>
        public static Func<T, object> GetSortOrderExression<T>(this string fieldName)
        {
            if (String.IsNullOrEmpty(fieldName))
                return null;

            var t = typeof(T);
            var normalizedFieldName = fieldName.Replace("_", string.Empty);
            var property = t.GetProperties().FirstOrDefault(p => p.Name.Equals(normalizedFieldName, StringComparison.OrdinalIgnoreCase));
            if (property == null)
                return null;

            return data => property.GetValue(data, null);
        }

        /// <summary>Order collection by field name and specified sort order</summary>
        /// <remarks>Field names are checked with ignore-case flag</remarks>
        public static IEnumerable<T> SortBy<T>(this IEnumerable<T> source, SortParam sort)
            where T : class
        {
            var result = sort.CustomExpression != null
                ? sort.CustomExpression(source) as IOrderedEnumerable<T>
                : source.OrderBy(a => 1);

            var exp = sort.Field.GetSortOrderExression<T>();
            if (exp == null)
                return result;

            return "desc".Equals(sort.Order, StringComparison.OrdinalIgnoreCase)
                ? result.ThenByDescending(exp)
                : result.ThenBy(exp);
        }

        /// <summary>Paginate collection by selected start and size</summary>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> PageBy<T>(this IEnumerable<T> source, PagingParam paging, out int total)
            where T : class
        {
            var start = (paging.Page - 1) * paging.Size;
            if (start < 0)
                start = 0;

            total = source.Count();

            var end = Math.Min(paging.Size, total - start);

            return source
                .Skip(start)
                .Take(end);
        }

        /// <summary>Filter collection by selected criterias</summary>
        public static IEnumerable<T> FilterBy<T>(this IEnumerable<T> source, IQueryFilter queryFilter)
            where T : class, IAsset, IFilterable
            => source.Where(i => i.ApplyFilter(queryFilter));
    }
}