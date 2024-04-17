using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using z5.ms.common.abstractions;
using Microsoft.Extensions.Logging;
using z5.ms.common.infrastructure.geoip;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.identity;
using z5.ms.infrastructure.user.repositories;
using z5.ms.infrastructure.user.services;
using RegisterSocialUserCommand = z5.ms.domain.user.user.RegisterSocialUserCommand;
using System;

namespace z5.ms.infrastructure.user.user
{
    /// <summary>Handler to registering user with social media access token command</summary>
    public class RegisterSocialUserCommandHandler : IAsyncRequestHandler<RegisterSocialUserCommand, Result<OAuthToken>>
    {
        private readonly ISocialAuthenticationService _authenticationService;
        private readonly IAuthTokenService _tokenService;
        private readonly ILogger _logger;


        /// <inheritdoc />
        public RegisterSocialUserCommandHandler(ISocialAuthenticationService authenticationService, IAuthTokenService tokenService, ILoggerFactory loggerFactory)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _logger = loggerFactory.CreateLogger("Registering user with social media");
        }
        
        /// <inheritdoc />
        public async Task<Result<OAuthToken>> Handle(RegisterSocialUserCommand command)
        {
            var registerResult = await _authenticationService.Register(command);
            var sourceapp = command.Additional.ContainsKey("sourceapp") ? command.Additional["sourceapp"].ToString() : (command.Additional.ContainsKey("Sourceapp") ? command.Additional["Sourceapp"].ToString() : string.Empty);
            if (!registerResult.Success)
            {
                _logger.LogInformation($"FAILTURE | {DateTime.UtcNow} | {registerResult.StatusCode} | {registerResult.Success} |{ registerResult.Error.Message}|{sourceapp}");
                return Result<OAuthToken>.FromError(registerResult);
            }
            _logger.LogInformation($"SUCCESS | {DateTime.UtcNow} | {registerResult.StatusCode} | {registerResult.Success} |{sourceapp}");
            return await _tokenService.GetJwtToken(registerResult.Value.Id, command.RegistrationCountry, command.Refresh);
        }
    }
}