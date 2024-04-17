using System.Threading.Tasks;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.common.notifications;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.services;

namespace z5.ms.infrastructure.user.user
{
    /// <summary>Handler for sending confirmation to mobile number again command</summary>
    public class ResendConfirmationSmsCommandHandler : IAsyncRequestHandler<ResendConfirmationSmsCommand, Result<Success>>
    {
        private readonly IConfirmationService _confirmationService;
        private readonly INotificationClient _notificationClient;

        /// <inheritdoc />
        public ResendConfirmationSmsCommandHandler(IConfirmationService confirmationService, INotificationClient notificationClient)
        {
            _confirmationService = confirmationService;
            _notificationClient = notificationClient;
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Handle(ResendConfirmationSmsCommand command)
        {
            var userResult = await _confirmationService.ReCreateConfirmationCode(command);
            if (!userResult.Success)
                return Result<Success>.FromError(userResult.Error);

            var user = userResult.Value;
            _notificationClient.SendRegistrationActivationSms(user.Mobile, user.MobileConfirmationKey, user.RegistrationCountry);

            return Result<Success>.FromValue(new Success
            {
                Code = 1,
                Message = "Confirmation sms has been sent to queue"
            });
        }
    }
}