using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.watchlist
{
    /// <summary>Command for adding a new catalog item to user's watchlist</summary>
    public class AddWatchlistCommand : WatchlistCommandBase
    {
    }
    
    /// <summary>Validator for adding a new catalog item to user's watchlist command</summary>
    public class AddWatchListCommandValidator : WatchlistCommandBaseValidator<AddWatchlistCommand>
    {
        /// <inheritdoc />
        public AddWatchListCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
        }
    }
}