using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.watchhistory
{
    /// <summary>Query to get watchhistory of user</summary>
    public class GetWatchhistoryQuery : IRequest<Result<List<CatalogItem>>>
    {
        /// <summary>User Id</summary>
        [JsonProperty("user_id", Required = Required.Always)]
        [Required]
        public Guid? UserId { get; set; }
    }
    
    /// <summary>Validator for watchhistory query</summary>
    public class GetWatchhistoryQueryValidator : AbstractValidator<GetWatchhistoryQuery>
    {
        /// <inheritdoc />
        public GetWatchhistoryQueryValidator (IOptions<UserErrors> errors)
        {
            RuleFor(c => c.UserId).Must(id => id.HasValue).WithMessage(errors.Value.MissingUserId.Message);
        }
    }
}