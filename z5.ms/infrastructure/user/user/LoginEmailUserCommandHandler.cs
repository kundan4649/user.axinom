using System.Threading.Tasks;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.identity;
using z5.ms.infrastructure.user.services;

namespace z5.ms.infrastructure.user.user
{
    /// <summary>Handler for logging user in with email address command</summary>
    public class LoginEmailUserCommandHandler : IAsyncRequestHandler<LoginEmailUserCommand, Result<OAuthToken>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthTokenService _tokenService;

        /// <inheritdoc />
        public LoginEmailUserCommandHandler(IAuthenticationService authenticationService, IAuthTokenService tokenService)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
        }

        /// <inheritdoc />
        public async Task<Result<OAuthToken>> Handle(LoginEmailUserCommand command)
        {
            var loginResult = await _authenticationService.Login(command);
            if (!loginResult.Success)
                return Result<OAuthToken>.FromError(loginResult);

            return await _tokenService.GetJwtToken(loginResult.Value.Id, command.Country, command.Refresh);
        }
    }
}