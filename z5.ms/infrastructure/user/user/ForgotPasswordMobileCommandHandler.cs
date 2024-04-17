using System.Threading.Tasks;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.services;

namespace z5.ms.infrastructure.user.user
{
    /// <summary>Command Handler for sending a code to replace forgotten password with new one</summary>
    public class ForgotPasswordMobileCommandHandler : IAsyncRequestHandler<ForgotPasswordMobileCommand, Result<Success>>
    {
        private readonly IPasswordService _passwordService;


        /// <inheritdoc />
        public ForgotPasswordMobileCommandHandler(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(ForgotPasswordMobileCommand command)
        {
            return await _passwordService.SendPasswordResetNotification(command);
        }
    }
}