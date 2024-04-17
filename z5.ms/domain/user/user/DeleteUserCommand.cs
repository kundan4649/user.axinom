using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.user
{
    /// <summary>Command for marking user as deleted</summary>
    public class DeleteUserCommand : UserCommandBase
    {
    }

    /// <summary>Validator for delete user command</summary>
    public class DeleteUserCommandValidator : UserCommandBaseValidator<DeleteUserCommand>
    {
        /// <inheritdoc />
        public DeleteUserCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
        }
    }
}