using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>Command for creating a new user with mobile number activation</summary>
    public class RegisterMobileUserCommand : RegisterUserCommand, IRequest<Result<Success>>
    {
        /// <inheritdoc />
        [JsonIgnore]
        public override AuthenticationMethod Type => AuthenticationMethod.Mobile;

        /// <summary>The mobile number of the user</summary>
        [JsonProperty("mobile", Required = Required.Always)]
        [Required]
        public string Mobile { get; set; }

        /// <summary>The password of the user.</summary>
        [JsonProperty("password", Required = Required.Always)]
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 6)]
        public string Password { get; set; }

        /// <summary>The first name of the user</summary>
        [JsonProperty("first_name", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string FirstName { get; set; }

        /// <summary>The first name of the user</summary>
        [JsonProperty("last_name", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string LastName { get; set; }
    }

    /// <summary>Validator for registering user with mobile number activation</summary>
    public class RegisterMobileUserCommandValidator : AbstractValidator<RegisterMobileUserCommand>
    {
        /// <inheritdoc />
        public RegisterMobileUserCommandValidator(IOptions<UserErrors> errors)
        {
            // Below Code Commented : Code Fix For - https://asiatvusa.atlassian.net/browse/Z5BE-46 - Code is getting failed for Mobile number validations using Phonenumbers.dll external library

            //RuleFor(c => c.Mobile).Must(ValidateContactDetails.IsPhoneNumber).WithMessage(errors.Value.InvalidPhone.Message);

            // Code fixed for Z%BE-46 Indian Mobile NUmber using Country Code - this will not accept Short numbers ( Ex: 123,8967 etc //
            // Code Review - Done
            RuleFor(c => c.Mobile).Must(ValidateContactDetails.IsMobileNumberforRegistration).WithMessage(errors.Value.InvalidPhone.Message);
            RuleFor(c => c.Password).Must(p => !string.IsNullOrWhiteSpace(p) && p.Trim().Length >= 6).WithMessage(errors.Value.InvalidPassword.Message);
        }

    }
}