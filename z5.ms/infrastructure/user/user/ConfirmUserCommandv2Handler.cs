using System.Threading.Tasks;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.services;

namespace z5.ms.infrastructure.user.user
{
    /// <summary>Handler for confirming user email address command</summary>
    public class ConfirmUserCommandv2Handler : IAsyncRequestHandler<ConfirmUserCommandv2, Result<Success>>
    {
        private readonly IConfirmationService _confirmationService;

        /// <inheritdoc />
        public ConfirmUserCommandv2Handler(IConfirmationService confirmationService)
        {
            _confirmationService = confirmationService;
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Handle(ConfirmUserCommandv2 command)
        {
            return await _confirmationService.Confirmv2(command);
        }
    }
}