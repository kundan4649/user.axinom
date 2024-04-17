using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.customer
{
    /// <summary>Customer creation request parameters</summary>
    public class CreateCustomerCommand : RegisterUserCommand, IRequest<Result<Customer>>
    {
        /// <summary>The email address of the user</summary>
        [JsonProperty("email", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        /// <summary>The mobile phone number of the user</summary>
        [JsonProperty("mobile", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Mobile { get; set; }

        /// <summary>The password of the user.</summary>
        [JsonProperty("password", Required = Required.Always)]
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 6)]
        public string Password { get; set; }

        /// <summary>The first name of the user</summary>
        [JsonProperty("first_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        /// <summary>The first name of the user</summary>
        [JsonProperty("last_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        /// <summary>The system of the user</summary>
        [JsonProperty("system", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string System { get; set; } = "Internal";
    }

    /// <summary>Validator for create customer command</summary>
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        /// <inheritdoc />
        public CreateCustomerCommandValidator(IOptions<UserErrors> errors)
        {
            RuleFor(m => m.Email).Must(s => s.IsNullOrEmpty() || ValidateContactDetails.IsEmail(s))
                .WithMessage(errors.Value.InvalidEmail.Message);

            // RuleFor(m => m.Mobile).Must(s => s.IsNullOrEmpty() || ValidateContactDetails.IsPhoneNumber(s))
            //     .WithMessage(errors.Value.InvalidPhone.Message);

            RuleFor(m => m.Mobile).Must(s => s.IsNullOrEmpty() || ValidateContactDetails.IsMobileNumber(s))
                 .WithMessage(errors.Value.InvalidPhone.Message);

            RuleFor(m => m.Password).Must(s => !string.IsNullOrWhiteSpace(s) && s.Trim().Length >= 6)
                .WithMessage(errors.Value.InvalidPassword.Message);

            RuleFor(m => m.RegistrationCountry).Must(s => s.IsNullOrEmpty() || s.Length == 2)
                .WithMessage(errors.Value.InvalidCountry.Message);
        }
    }
}