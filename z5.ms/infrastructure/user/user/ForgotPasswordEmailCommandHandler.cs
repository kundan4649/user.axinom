using System.Threading.Tasks;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.services;

namespace z5.ms.infrastructure.user.user
{
    /// <summary>Command Handler for sending a code to replace forgotten password with new one</summary>
    public class ForgotPasswordEmailCommandHandler : IAsyncRequestHandler<ForgotPasswordEmailCommand, Result<Success>>
    {
        private readonly IPasswordService _passwordService;

        /// <inheritdoc />
        public ForgotPasswordEmailCommandHandler(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(ForgotPasswordEmailCommand command)
        {
            return await _passwordService.SendPasswordResetNotification(command);
        }
    }
}