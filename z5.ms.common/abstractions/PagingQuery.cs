using System;
using System.Runtime.Serialization;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace z5.ms.common.abstractions
{
    /// <summary>Get sorted and paginated list of TAsset</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PagingQuery<TPagedResult, TSortingEnum> : IRequest<Result<TPagedResult>>
    {
        /// <summary>The page of the result set (one for the first page)</summary>
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1; //default = 1, minimal = 1

        /// <summary>How many items should be returned per page</summary>
        [FromQuery(Name = "page_size")]
        public int PageSize { get; set; } = 25; //default: 25, minimum: 1, maximum: 100

        /// <summary>The name of the field by which to sort</summary>
        [FromQuery(Name = "sort_by_field")]
        public TSortingEnum SortByField { get; set; } // enum: title, date

        /// <summary>The sort order of the "sort by field"</summary>
        [FromQuery(Name = "sort_order")]
        public SortOrder SortOrder { get; set; } //asc, dec
    }

    /// <inheritdoc />
    /// <summary>Get paged list query parameters validator</summary>
    public class PagingQueryValidator<TQuery, TSorting, TResult> : AbstractValidator<TQuery>
        where TQuery : PagingQuery<TResult, TSorting>
    {
        /// <inheritdoc />
        protected PagingQueryValidator()
        {
            RuleFor(m => m.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be equal or greater than 1");

            RuleFor(m => m.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be an integer from 1 to 100 inclusively");

            RuleFor(m => m.SortByField)
                .Must(s => Enum.IsDefined(s.GetType(), s)).WithMessage("Unsupported sort field name");

            RuleFor(m => m.SortOrder)
                .Must(s => Enum.IsDefined(s.GetType(), s)).WithMessage("Unsupported sort order name");
        }
    }

    /// <summary>The sort order of the "sort by field".</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortOrder
    {
        /// <summary>Ascending</summary>
        [EnumMember(Value = "asc")]
        Asc = 0,

        /// <summary>Descending</summary>
        [EnumMember(Value = "desc")]
        Desc = 1
    }
}