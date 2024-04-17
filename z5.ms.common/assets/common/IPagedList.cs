namespace z5.ms.common.assets.common
{
    /// <summary>Interface for paged results</summary>
    public interface IPagedList
    {
        /// <summary>Current page of the result set (one based)</summary>
        int Page { get; set; }
        
        /// <summary>Max amount of items returned per page</summary>
        int PageSize { get; set; }
    }
}