using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.watchlist
{
    /// <summary>Command for updating a catalog item to user's watchlist</summary>
    public class UpdateWatchlistCommand : WatchlistCommandBase
    {
    }
    
    /// <summary>Validator for updating a catalog item to user's watchlist command</summary>
    public class UpdateWatchListCommandValidator : WatchlistCommandBaseValidator<UpdateWatchlistCommand>
    {
        /// <inheritdoc />
        public UpdateWatchListCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
        }
    }
}