using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using z5.ms.common.abstractions;
using z5.ms.common.attributes;
using z5.ms.domain.subscription.voucher.viewmodels;

namespace z5.ms.domain.subscription.voucher.queries
{
    /// <summary>Get paginated list of voucher batches</summary>
    public class GetVoucherBatchesQuery : IRequest<Result<VoucherBatches>>
    {
        /// <summary>Filter by a specific subscription plan.</summary>
        [FromQuery(Name = "subscription_plan_id")]
        public string SubscriptionPlanId { get; set; }

        /// <summary>The name of the field by which to sort</summary>
        [FromQueryEnumMember(Name = "sort_by_field")]
        public VoucherBatchSortByField SortByField { get; set; } = VoucherBatchSortByField.BatchId;

        /// <summary>The sort order of the "sort by field"</summary>
        [FromQuery(Name = "sort_order")]
        public SortOrder SortOrder { get; set; } = SortOrder.Asc;

        /// <summary>The page of the result set (one based).</summary>
        [FromQuery(Name = "page")]
        public int? Page { get; set; } = 1;

        /// <summary>How many vouchers should be returned per page.</summary>
        [FromQuery(Name = "page_size")]
        public int? PageSize { get; set; } = 25;
    }

    /// <summary>GetVoucherBatches query parameters validator</summary>
    public class GetVoucherBatchesQueryValidator : AbstractValidator<GetVoucherBatchesQuery>
    {
        /// <inheritdoc />
        public GetVoucherBatchesQueryValidator()
        {
            RuleFor(c => c.SubscriptionPlanId).NotEmpty();
            RuleFor(c => c.Page).GreaterThanOrEqualTo(1).WithMessage("Page size must be equal of greater than 1");
            RuleFor(c => c.PageSize).InclusiveBetween(1, 100).WithMessage("Page size must be an integer from 1 to 100 inclusively");
        }
    }
}

