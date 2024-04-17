using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.settings
{
    /// <summary>Command for adding a new setting</summary>
    public class AddSettingCommand : SettingCommandBase
    {    
    }

    /// <summary>Validator for adding a new setting command</summary>
    public class AddSettingCommandValidator : SettingCommandBaseValidator<AddSettingCommand>
    {
        /// <inheritdoc />
        public AddSettingCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
        }
    }
}