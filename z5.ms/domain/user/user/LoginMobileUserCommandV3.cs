using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    public class LoginMobileUserCommandV3 : LoginUserCommand, IRequest<Result<OAuthToken>>
    {
        /// <inheritdoc />
        [JsonIgnore]
        public override AuthenticationMethod Type => AuthenticationMethod.Mobile;

        /// <summary>The mobule number of the user.</summary>
        [JsonProperty("mobile", Required = Required.Always)]
        [Required]
        public string Mobile { get; set; }

        /// <summary>The password of the user.</summary>
        [JsonProperty("password", Required = Required.Always)]
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 6)]
        public string Password { get; set; }

        /// <summary>The cttl for the user.</summary>
        [JsonProperty("cttl", Required = Required.AllowNull)]
        public int Cttl { get; set; }

        /// <summary>The deviceId for the user.</summary>
        [JsonProperty("deviceId", Required = Required.AllowNull)]
        public string DeviceId { get; set; }
    }

    /// <summary>Command for logging in user with mobile phone number and password</summary>
    public class LoginMobileUserCommandV3Validator : AbstractValidator<LoginMobileUserCommandV3>
    {
        /// <inheritdoc />
        public LoginMobileUserCommandV3Validator(IOptions<UserErrors> errors)
        {
            //RuleFor(c => c.Mobile).Must(ValidateContactDetails.IsPhoneNumber).WithMessage(errors.Value.InvalidPhone.Message);
            RuleFor(c => c.Mobile).Must(ValidateContactDetails.IsMobileNumber).WithMessage(errors.Value.InvalidPhone.Message);
            RuleFor(c => c.Password).Must(p => !string.IsNullOrWhiteSpace(p) && p.Trim().Length >= 6).WithMessage(errors.Value.InvalidPassword.Message);
        }
    }
}