using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.favorites;

namespace z5.ms.infrastructure.user.favorites
{
    /// <summary>Update user's single favorite item command handler</summary>
    public class UpdateFavoriteCommandHandler : IAsyncRequestHandler<UpdateFavoriteCommand, Result<Success>>
    {
        private readonly IFavoritesRepository _repository;

        /// <inheritdoc />
        public UpdateFavoriteCommandHandler(IFavoritesRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Handle(UpdateFavoriteCommand message)
        {
            var result = await _repository.UpdateItemAsync(message.UserId.Value, message.Item);
            if (result.Success)
                return Result<Success>.FromValue(new Success());

            return Result<Success>.FromError(result.Error);
        }
    }
}