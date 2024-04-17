using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.watchhistory
{
    /// <summary>Command for deleting a catalog item from user's watchhistory</summary>
    public class DeleteWatchhistoryCommand : WatchhistoryCommandBase
    {
    }
    
    /// <summary>Validator for deleting a catalog item from user's watchhistory command</summary>
    public class DeleteWatchhistoryCommandValidator : WatchhistoryCommandBaseValidator<DeleteWatchhistoryCommand>
    {
        /// <inheritdoc />
        public DeleteWatchhistoryCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
        }
    }
}