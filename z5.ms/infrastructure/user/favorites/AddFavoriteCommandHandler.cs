using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.favorites;

namespace z5.ms.infrastructure.user.favorites
{
    /// <summary>Handler for add favorite command</summary>
    public class AddFavoriteCommandHandler : IAsyncRequestHandler<AddFavoriteCommand, Result<Success>>
    {
        private readonly IFavoritesRepository _repository;
        
        /// <inheritdoc />
        public AddFavoriteCommandHandler(IFavoritesRepository repository)
        {
            _repository = repository;
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(AddFavoriteCommand message)
        {
            var result = await _repository.AddItemAsync(message.UserId.Value, message.Item);
            if (result.Success)
                return Result<Success>.FromValue(new Success());
            
            return Result<Success>.FromError(result.Error);
        }
    }
}