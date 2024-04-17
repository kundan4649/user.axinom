using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using z5.ms.common.abstractions;
using z5.ms.common.notifications;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.services;

namespace z5.ms.infrastructure.user.user
{
    /// <summary>Handler for sending confirmation to email address again command</summary>
    public class ResendConfirmationEmailCommandHandler : IAsyncRequestHandler<ResendConfirmationEmailCommand, Result<Success>>
    {
        private readonly IConfirmationService _confirmationService;
        private readonly INotificationClient _notificationClient;
        
        /// <inheritdoc />
        public ResendConfirmationEmailCommandHandler(IConfirmationService confirmationService, INotificationClient notificationClient)
        {
            _confirmationService = confirmationService;
            _notificationClient = notificationClient;
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(ResendConfirmationEmailCommand command)
        {
            var userResult = await _confirmationService.ReCreateConfirmationCode(command);
            if (!userResult.Success)
                return Result<Success>.FromError(userResult.Error);

            var user = userResult.Value;
            _notificationClient.SendRegistrationActivationEmail(user.Email, user.EmailConfirmationKey, user.RegistrationCountry);

            return Result<Success>.FromValue(new Success
            {
                Code = 1,
                Message = "Confirmation email has been sent to queue"
            });
        }
    }
}