using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;

namespace z5.ms.domain.user.user
{
    public class ResetPasswordCommandv2 : UserCommandBase
    {
        /// <summary>The random code that was sent to the users email address or mobile phone</summary>
        [JsonProperty("code", Required = Required.Always)]
        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string Code { get; set; }

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
        public RecreateType Type { get; set; }

        [JsonIgnore]
        /// <summary>Raw body</summary>
        public string RawRequest { get; set; }
    }

    public enum RecreateType
    {
        [EnumMember(Value = "Email")]
        Email,

        [EnumMember(Value = "Mobile")]
        Mobile
    }

    /// <summary>Validator for changing user's password with reset code</summary>
    public class ResetPasswordCommandv2Validator : AbstractValidator<ResetPasswordCommandv2>
    {
        /// <inheritdoc />
        public ResetPasswordCommandv2Validator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.Code)
                .Must(code => !string.IsNullOrWhiteSpace(code) && code.Trim().Length >= 4 && code.Trim().Length <= 20)
                .WithMessage(errors.Value.RecreatePasswordCodeInvalid.Message);

            RuleFor(c => c.NewPassword).Must(p => !string.IsNullOrWhiteSpace(p) && p.Trim().Length >= 6)
                .WithMessage(errors.Value.InvalidPassword.Message);

            RuleFor(c => new { c.RecipientAddress, c.Type }).Must(x => (x.Type == RecreateType.Mobile && !string.IsNullOrWhiteSpace(x.RecipientAddress) && (x.RecipientAddress.Trim().Length == 10 || x.RecipientAddress.Trim().Length == 12)) || (x.Type == RecreateType.Email && Regex.IsMatch(x.RecipientAddress, @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$"))).WithMessage(p => p.Type == RecreateType.Mobile ? errors.Value.InvalidPhone.Message : errors.Value.InvalidEmail.Message);
        }
    }
}
