using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>Command to send random code to user that can be used for change forgotten password</summary>
    public class ForgotPasswordMobileCommand : ForgotPasswordCommand, IRequest<Result<Success>>
    {
        /// <summary>Type of the authentication method</summary>
        [JsonIgnore]
        public override AuthenticationMethod Type => AuthenticationMethod.Mobile;

        /// <summary>The mobile number of the user</summary>
        [JsonProperty("mobile", Required = Required.Always)]
        [Required]
        public string Mobile { get; set; }
    }

    /// <summary>Validator for sending password change code command</summary>
    public class ForgotPasswordMobileCommandValidator : AbstractValidator<ForgotPasswordMobileCommand>
    {
        /// <inheritdoc />
        public ForgotPasswordMobileCommandValidator(IOptions<UserErrors> errors)
        {
            //RuleFor(c => c.Mobile).Must(ValidateContactDetails.IsPhoneNumber).WithMessage(errors.Value.InvalidPhone.Message);

            RuleFor(c => c.Mobile).Must(ValidateContactDetails.IsMobileNumber).WithMessage(errors.Value.InvalidPhone.Message);
            
        }
    } 
}