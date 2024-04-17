using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.favorites
{
    /// <summary>Command to update a favorite item</summary>
    public class UpdateFavoriteCommand : FavoriteCommandBase
    {
    }

    /// <summary>Validator for updating a favorite item</summary>
    public class UpdateFavoriteCommandValidator : FavoriteCommandBaseValidator<UpdateFavoriteCommand>
    {
        /// <inheritdoc />
        public UpdateFavoriteCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
        }
    }
}