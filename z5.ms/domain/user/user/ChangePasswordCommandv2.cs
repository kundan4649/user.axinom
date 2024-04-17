using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;

namespace z5.ms.domain.user.user
{
    /// <summary>Command to change user password</summary>
    public class ChangePasswordCommandv2 : UserCommandBase
    {
        /// <summary>The old password of the user</summary>
        [JsonProperty("old_password", Required = Required.Always)]
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 6)]
        public string OldPassword { get; set; }

        /// <summary>The new password of the user</summary>
        [JsonProperty("new_password", Required = Required.Always)]
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 6)]
        public string NewPassword { get; set; }

        /// <summary>The email or mobile of the user</summary>
        [JsonProperty("recipient_address", Required = Required.Always)]
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 6)]
        public string RecipientAddress { get; set; }

        [JsonIgnore]
        /// <summary>Raw body</summary>
        public string RawRequest { get; set; }
    }

    /// <summary>Validator for change user password command </summary>
    public class ChangePasswordCommandv2Validator : AbstractValidator<ChangePasswordCommandv2>
    {
        /// <inheritdoc />
        public ChangePasswordCommandv2Validator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.UserId).Must(id => id != Guid.Empty).WithMessage(errors.Value.MissingUserId.Message);

            RuleFor(c => c.NewPassword).Must(p => !string.IsNullOrWhiteSpace(p) && p.Trim().Length >= 6)
                .WithMessage(errors.Value.InvalidPassword.Message);

            RuleFor(c => c.OldPassword).Must(p => !string.IsNullOrWhiteSpace(p) && p.Trim().Length >= 6)
                .WithMessage(errors.Value.InvalidPassword.Message);

            RuleFor(c => c.RecipientAddress).Must(p => !string.IsNullOrWhiteSpace(p) && p.Trim().Length >= 6)
                .WithMessage(errors.Value.UserNotFound.Message);
        }
    }
}