using System.Threading.Tasks;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.common.notifications;
using Microsoft.Extensions.Logging;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.services;
using System;

namespace z5.ms.infrastructure.user.user
{
    /// <summary>Handler for creating a new user with mobile registration command</summary>
    public class RegisterMobileUserCommandHandler : IAsyncRequestHandler<RegisterMobileUserCommand, Result<Success>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly INotificationClient _notificationClient;
        private readonly ILogger _logger;

        /// <inheritdoc />
        public RegisterMobileUserCommandHandler(IAuthenticationService authenticationService, INotificationClient notificationClient, ILoggerFactory loggerFactory)
        {
            _notificationClient = notificationClient;
            _authenticationService = authenticationService;
            _logger = loggerFactory.CreateLogger("Registration for success and failure");
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(RegisterMobileUserCommand command)
        {
            _logger.LogInformation($"Registration: Handler initiated | Mobile: {command.Mobile}");

            var registerResult = await _authenticationService.Register(command);
            var sourceapp = command.Additional.ContainsKey("sourceapp") ? command.Additional["sourceapp"].ToString() : (command.Additional.ContainsKey("Sourceapp") ? command.Additional["Sourceapp"].ToString() : string.Empty);
            if (!registerResult.Success)
            {
                _logger.LogInformation($"FAILTURE | {DateTime.UtcNow} | {registerResult.StatusCode} | {registerResult.Success} | {registerResult.Error.Message} |{sourceapp}|");
                return Result<Success>.FromError(registerResult);
            }
            _logger.LogInformation($"SUCCESS | {DateTime.UtcNow} | {registerResult.StatusCode} |  {registerResult.Success}|{command.Message} |{sourceapp}|");
            _notificationClient.SendRegistrationActivationSms(registerResult.Value.Mobile, registerResult.Value.MobileConfirmationKey, registerResult.Value.RegistrationCountry);

            return Result<Success>.FromValue(new Success
            {
                Code = 1,
                Message = "Registration successful"
            });
        }
    }
}