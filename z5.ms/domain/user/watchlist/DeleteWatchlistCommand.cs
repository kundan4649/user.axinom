using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.watchlist
{
    /// <summary>Command for deleting a catalog item to user's watchlist</summary>
    public class DeleteWatchlistCommand : WatchlistCommandBase
    {
    }
    
    /// <summary>Validator for deleting a new catalog item to user's watchlist command</summary>
    public class DeleteWatchListCommandValidator : WatchlistCommandBaseValidator<DeleteWatchlistCommand>
    {
        /// <inheritdoc />
        public DeleteWatchListCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
        }
    }
}