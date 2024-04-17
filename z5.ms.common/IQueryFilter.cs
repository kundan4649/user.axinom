using System;

namespace z5.ms.common
{
    /// <summary>Query filter parameter</summary>
    /// <remarks>Various asset types have various filter requirements, each IAsset implementation may define one or more query filters</remarks>
    public interface IQueryFilter
    {
        /// <summary>The desired language the feed metadata should have as two letter “ISO 639-1” code</summary>
        string Translation { get; }
    }
    
    /// <inheritdoc />
    /// <summary>Marker interface for search filters</summary>
    public interface ISearchFilter : IQueryFilter {}
    
    /// <summary>Query sort parameter</summary>
    public struct SortParam
    {
        /// <summary>The name of the field by which to sort</summary>
        /// <remarks>enum: title, date</remarks>
        public string Field { get; }

        /// <summary>The sort order of the "sort by field"</summary>
        /// <remarks>enum: asc, dec</remarks>
        public string Order { get; }

        /// <summary>Custom sorting expression</summary>
        /// <remarks>Custom expression has higher priority if provided</remarks>
        public Func<object, object> CustomExpression { get; }

        /// <summary>Public constructor</summary>
        public SortParam(string field = null, string order = null, Func<object, object> expression = null)
        {
            Field = field ?? "title";
            Order = order ?? "desc";
            CustomExpression = expression;
        }

        /// <summary>Public constructor</summary>
        public SortParam(Func<object, object> expression)
        {
            Field = null;
            Order = null;
            CustomExpression = expression;
        }
    }

    /// <summary>Query sort parameter</summary>
    public struct PagingParam
    {
        /// <summary>The page of the result set (one for the first page)</summary>
        /// <remarks>default = 1, minimal = 1</remarks>
        public int Page { get; }

        /// <summary>How many items should be returned per page</summary>
        /// <remarks>default: 25, minimum: 1, maximum: 100</remarks>
        public int Size { get; }

        /// <summary>Public constructor</summary>
        public PagingParam(int page = 1, int size = 25)
        {
            Page = page;
            Size = size;
        }
    }
}