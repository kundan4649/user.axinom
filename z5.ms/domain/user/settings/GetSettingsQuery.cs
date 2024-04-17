using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.settings
{
    /// <summary>Query for getting user's settings</summary>
    public class GetSettingsQuery : IRequest<Result<List<SettingItem>>>
    {
        /// <summary>User Id</summary>
        [JsonProperty("user_id", Required = Required.Always)]
        [Required]
        public Guid? UserId { get; set; }
    }

    /// <summary>Validator for get user's settings query</summary>
    public class GetSettingsQueryValidator : AbstractValidator<GetSettingsQuery>
    {
        /// <inheritdoc />
        public GetSettingsQueryValidator(IOptions<UserErrors> errors)
        {
            RuleFor(q => q.UserId).Must(id => id.HasValue).WithMessage(errors.Value.MissingCustomerId.Message);
        }
    }
}