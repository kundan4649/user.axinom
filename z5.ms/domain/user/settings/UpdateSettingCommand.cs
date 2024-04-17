using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.settings
{
    /// <summary>Command for updating a user's setting value</summary>
    public class UpdateSettingCommand : SettingCommandBase
    {
    }

    /// <summary>Validator for updating a user's setting value command</summary>
    public class UpdateSettingCommandValidation : SettingCommandBaseValidator<UpdateSettingCommand>
    {
        /// <inheritdoc />
        public UpdateSettingCommandValidation(IOptions<UserErrors> errors) : base(errors)
        {
        }
    }
}