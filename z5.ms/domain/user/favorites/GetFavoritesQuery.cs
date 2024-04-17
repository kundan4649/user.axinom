using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.favorites
{
    /// <summary>Query to fetch user's favorite catalog items</summary>
    public class GetFavoritesQuery : IRequest<Result<List<CatalogItem>>>
    {
        /// <summary>User Id</summary>
        [JsonProperty("user_id", Required = Required.Always)]
        [Required]
        public Guid? UserId { get; set; }
    }

    /// <summary>Validator for fetching user's favorite catalog items</summary>
    public class GetFavoritesQueryValidator : AbstractValidator<GetFavoritesQuery>
    {
        /// <inheritdoc />
        public GetFavoritesQueryValidator(IOptions<UserErrors> errors)
        {
            RuleFor(q => q.UserId).Must(id => id.HasValue).WithMessage(errors.Value.MissingCustomerId.Message);
        }
    }
}