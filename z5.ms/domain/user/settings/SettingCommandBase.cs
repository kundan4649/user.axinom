using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.settings
{
    /// <summary>Base command for different settings commands</summary>
    public class SettingCommandBase : IRequest<Result<Success>>
    {
        /// <summary>User Id</summary>
        [JsonProperty("user_id", Required = Required.Always)]
        [Required]
        public Guid? UserId { get; set; }
        
        /// <summary>Setting item</summary>
        [JsonProperty("setting_item", Required = Required.Always)]
        [Required]
        public SettingItem Item { get; set; }
    }

    /// <summary>Validator for base setting commands</summary>
    public class SettingCommandBaseValidator<TCommand> : AbstractValidator<TCommand> where TCommand : SettingCommandBase
    {
        /// <inheritdoc />
        public SettingCommandBaseValidator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.UserId).Must(id => id.HasValue).WithMessage(errors.Value.MissingCustomerId.Message);

            RuleFor(c => c.Item).Must(item => item != null).WithMessage(errors.Value.SettingsItemMissing.Message);
        }
    }
}