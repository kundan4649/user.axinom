using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.watchhistory
{
    /// <summary>Command for updating an existing catalog item in user's watch history</summary>
    public class UpdateWatchhistoryCommand : WatchhistoryCommandBase
    {
    }
    
    /// <summary>Validator for update an existing catalog item in user's watch history command</summary>
    public class UpdateWatchhistoryCommandValidator : WatchhistoryCommandBaseValidator<UpdateWatchhistoryCommand>
    {
        /// <inheritdoc />
        public UpdateWatchhistoryCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
        }
    }
}