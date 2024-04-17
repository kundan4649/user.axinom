using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>Command to send confirmation to mobile phone again</summary>
    public class ResendConfirmationSmsCommand : ResendConfirmationCommand, IRequest<Result<Success>>
    {
        /// <inheritdoc />
        [JsonIgnore]
        public override AuthenticationMethod Type => AuthenticationMethod.Mobile;

        /// <summary>Mobile number where confirmation mail is sent</summary>
        [JsonProperty("mobile", Required = Required.Always)]
        [FromBody]
        public string Mobile { get; set; }
    }

    /// <summary>Validator for sending confirmation to mobile phone again</summary>
    public class ResendConfirmationSmsCommandValidator : AbstractValidator<ResendConfirmationSmsCommand>
    {
        /// <inheritdoc />
        public ResendConfirmationSmsCommandValidator(IOptions<UserErrors> errors)
        {
            
            //RuleFor(c => c.Mobile).Must(ValidateContactDetails.IsPhoneNumber).WithMessage(errors.Value.InvalidPhone.Message);
            RuleFor(c => c.Mobile).Must(ValidateContactDetails.IsMobileNumber).WithMessage(errors.Value.InvalidPhone.Message);
        }
    }
}