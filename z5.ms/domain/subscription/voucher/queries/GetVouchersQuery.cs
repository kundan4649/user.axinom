using System;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using z5.ms.common.abstractions;
using z5.ms.domain.subscription.voucher.viewmodels;

namespace z5.ms.domain.subscription.voucher.queries
{
    /// <summary>Get paginated list of vouchers</summary>
    public class GetVouchersQuery : IRequest<Result<Vouchers>>
    {
        /// <summary>Filter the vouchers for a specific batch.</summary>
        [FromQuery(Name = "batch_id")]
        public string BatchId { get; set; }

        /// <summary>Name of the CMS user who initiated generating vouchers.</summary>
        [FromQuery(Name = "cms_user")]
        public string CmsUser { get; set; }

        /// <summary>Filter by a specific subscription plan.</summary>
        [FromQuery(Name = "subscription_plan_id")]
        public string SubscriptionPlanId { get; set; }

        /// <summary>Filter by the status of voucher (redeemed or not).</summary>
        [FromQuery(Name = "redeemed")]
        public bool? Redeemed { get; set; }

        /// <summary>Filter by date, only return vouchers that redeemed after this time.</summary>
        [FromQuery(Name = "redeemed_from")]
        public DateTime? RedeemedFrom { get; set; }

        /// <summary>Filter by date, only return vouchers that redeemed before this time.</summary>
        [FromQuery(Name = "redeemed_until")]
        public DateTime? RedeemedUntil { get; set; }

        /// <summary>The page of the result set (one based).</summary>
        [FromQuery(Name = "page")]
        public int? Page { get; set; } = 1;

        /// <summary>How many vouchers should be returned per page.</summary>
        [FromQuery(Name = "page_size")]
        public int? PageSize { get; set; } = 25;
    }

    /// <summary>GetVouchers query parameters validator</summary>
    public class GetVouchersQueryValidator : AbstractValidator<GetVouchersQuery>
    {
        /// <inheritdoc />
        public GetVouchersQueryValidator()
        {
            RuleFor(c => c.Page).GreaterThanOrEqualTo(1).WithMessage("Page size must be equal of greater than 1");
            //RuleFor(c => c.PageSize).InclusiveBetween(1, 100).WithMessage("Page size must be an integer from 1 to 100 inclusively");
        }
    }
}

