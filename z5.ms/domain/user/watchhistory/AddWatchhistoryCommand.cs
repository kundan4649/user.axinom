using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.watchhistory
{
    /// <summary>Command for adding a new catalog item to user's Watchhistory</summary>
    public class AddWatchhistoryCommand : WatchhistoryCommandBase
    {
    }
    
    /// <summary>Validator for adding a new catalog item to user's Watchhistory command</summary>
    public class AddWatchhistoryCommandValidator : WatchhistoryCommandBaseValidator<AddWatchhistoryCommand>
    {
        /// <inheritdoc />
        public AddWatchhistoryCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
        }
    }
}