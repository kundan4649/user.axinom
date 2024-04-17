using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;

namespace z5.ms.domain.user.user
{
    /// <summary>Command to change a user's password with sent reset code</summary>
    public class ResetPasswordCommand : UserCommandBase
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

        [JsonIgnore]
        /// <summary>Raw body</summary>
        public string RawRequest { get; set; }
    }

    /// <summary>Validator for changing user's password with reset code</summary>
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        /// <inheritdoc />
        public ResetPasswordCommandValidator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.Code)
                .Must(code => !string.IsNullOrWhiteSpace(code) && code.Trim().Length >= 4 && code.Trim().Length <= 20)
                .WithMessage(errors.Value.RecreatePasswordCodeInvalid.Message); 
            
            RuleFor(c => c.NewPassword).Must(p => !string.IsNullOrWhiteSpace(p) && p.Trim().Length >= 6)
                .WithMessage(errors.Value.InvalidPassword.Message);
        }
    }
}