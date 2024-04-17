using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.customer
{
    /// <summary>User creation using mobile phone number request parameters</summary>
    public class CreateCustomerMobileCommand : IRequest<Result<Customer>>
    {
        /// <summary>The mobile phone number of the user</summary>
        [JsonProperty("mobile", Required = Required.Always)]
        [Required]
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
        
        /// <summary>Request sender's IP address</summary>
        [JsonIgnore]
        public string IpAddress { get; set; }
    }
    
    /// <summary> Create customer with email command validator</summary>
    public class CreateCustomerMobileCommandValidator : AbstractValidator<CreateCustomerMobileCommand>
    {
        /// <inheritdoc />
        public CreateCustomerMobileCommandValidator(IOptions<UserErrors> errors)
        {
            RuleFor(m => m.Password).Must(s => !string.IsNullOrWhiteSpace(s) && s.Trim().Length >= 6)
                .WithMessage(errors.Value.InvalidPassword.Message);

            //RuleFor(m => m.Mobile).Must(ValidateContactDetails.IsPhoneNumber).WithMessage(errors.Value.InvalidPhone.Message);

            RuleFor(m => m.Mobile).Must(ValidateContactDetails.IsMobileNumber).WithMessage(errors.Value.InvalidPhone.Message);

        }
    }
}