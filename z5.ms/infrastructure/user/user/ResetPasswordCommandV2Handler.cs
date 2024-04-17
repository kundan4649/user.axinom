using System.Threading.Tasks;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.services;


namespace z5.ms.infrastructure.user.user
{
    /// <summary>Handler for changing a user's password with sent reset code command</summary>
    public class ResetPasswordCommandV2Handler : IAsyncRequestHandler<ResetPasswordCommandv2, Result<Success>>
    {
        private readonly IPasswordService _passwordService;

        /// <inheritdoc />
        public ResetPasswordCommandV2Handler(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Handle(ResetPasswordCommandv2 command)
        {
            return await _passwordService.ResetPasswordv2(command);
        }
    }
}
