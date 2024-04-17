using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.settings
{
    /// <summary>Command for deleting a user's setting </summary>
    public class DeleteSettingCommand : SettingCommandBase
    {
    }

    /// <summary>Validator for delete user's setting command</summary>
    public class DeleteSettingCommandValidator : SettingCommandBaseValidator<DeleteSettingCommand>
    {
        /// <inheritdoc />
        public DeleteSettingCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
        }
    }
}