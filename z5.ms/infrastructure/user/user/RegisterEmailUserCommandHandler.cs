using System.Threading.Tasks;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.common.notifications;
using z5.ms.domain.user.user;
using Microsoft.Extensions.Logging;
using z5.ms.infrastructure.user.services;
using System;
using System.Linq;

namespace z5.ms.infrastructure.user.user
{
    /// <summary>Handler for creating a new user with email registration command</summary>
    public class RegisterEmailUserCommandHandler : IAsyncRequestHandler<RegisterEmailUserCommand, Result<Success>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly INotificationClient _notificationClient;
        private readonly ILogger _logger;

        /// <inheritdoc />
        public RegisterEmailUserCommandHandler(IAuthenticationService authenticationService, INotificationClient notificationClient, ILoggerFactory loggerFactory)
        {
            _notificationClient = notificationClient;
            _authenticationService = authenticationService;
            _logger = loggerFactory.CreateLogger("Registration for success and fuilure");
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(RegisterEmailUserCommand command)
        {
            var user = await _authenticationService.Register(command);
            var sourceapp = command.Additional.ContainsKey("sourceapp") ? command.Additional["sourceapp"].ToString() : (command.Additional.ContainsKey("Sourceapp") ? command.Additional["Sourceapp"].ToString() : string.Empty);
            if (!user.Success)
            {
                _logger.LogInformation($"FAILTURE | {DateTime.UtcNow} | {user.StatusCode} | {user.Success} | {user.Error.Message} | {sourceapp} |");
                return Result<Success>.FromError(user);
            }
            _logger.LogInformation($"SUCCESS | {DateTime.UtcNow} | {user.StatusCode} | {user.Success} | {command.Message} | {sourceapp} |");
            _notificationClient.SendRegistrationActivationEmail(user.Value.Email, user.Value.EmailConfirmationKey, user.Value.RegistrationCountry);

            return Result<Success>.FromValue(new Success
            {
                Code = 1,
                Message = "Registration successful"
            });
        }
    }
}