using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.watchlist
{
    /// <summary>Query to get watchlist of user</summary>
    public class GetWatchlistQuery : IRequest<Result<List<CatalogItem>>>
    {
        /// <summary>User Id</summary>
        [JsonProperty("user_id", Required = Required.Always)]
        [Required]
        public Guid? UserId { get; set; }
    }
    
    /// <summary>Validator for watchlist query</summary>
    public class GetWatchlistQueryValidator : AbstractValidator<GetWatchlistQuery>
    {
        /// <inheritdoc />
        public GetWatchlistQueryValidator (IOptions<UserErrors> errors)
        {
            RuleFor(c => c.UserId).Must(id => id.HasValue).WithMessage(errors.Value.MissingUserId.Message);
        }
    }
}