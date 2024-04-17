using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.favorites
{
    /// <summary>Command to add a favorite item</summary>
    public class AddFavoriteCommand : FavoriteCommandBase {}

    /// <summary>Validator for add a favorite item command</summary>
    public class AddFavoriteCommandValidator : FavoriteCommandBaseValidator<AddFavoriteCommand>
    {
        /// <inheritdoc />
        public AddFavoriteCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
        }
    }
}