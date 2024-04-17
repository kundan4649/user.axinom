using System.Threading.Tasks;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.services;

namespace z5.ms.infrastructure.user.user
{
    /// <summary>Handler for change user password command</summary>
    public class ChangePasswordCommandHandler : IAsyncRequestHandler<ChangePasswordCommand, Result<Success>>
    {
        private readonly IPasswordService _passwordService;
        
        /// <inheritdoc />
        public ChangePasswordCommandHandler(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(ChangePasswordCommand command)
        {
            return await _passwordService.ChangePassword(command);
        }
    }
}