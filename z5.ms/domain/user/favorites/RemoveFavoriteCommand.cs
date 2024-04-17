using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.favorites
{
    /// <summary>Command to remove a favorite item</summary>
    public class RemoveFavoriteCommand : FavoriteCommandBase
    {
    }

    /// <summary>Validator for removing a favorite item</summary>
    public class RemoveFavoriteCommandValidator : FavoriteCommandBaseValidator<RemoveFavoriteCommand>
    {
        /// <inheritdoc />
        public RemoveFavoriteCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
        }
    }
}