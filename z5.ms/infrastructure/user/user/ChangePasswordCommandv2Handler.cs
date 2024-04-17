using System.Threading.Tasks;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.services;

namespace z5.ms.infrastructure.user.user
{
    /// <summary>Handler for change user password command</summary>
    public class ChangePasswordCommandv2Handler : IAsyncRequestHandler<ChangePasswordCommandv2, Result<Success>>
    {
        private readonly IPasswordService _passwordService;

        /// <inheritdoc />
        public ChangePasswordCommandv2Handler(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Handle(ChangePasswordCommandv2 command)
        {
            return await _passwordService.ChangePasswordv2(command);
        }
    }
}