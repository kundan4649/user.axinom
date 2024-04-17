using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.watchlist;

namespace z5.ms.infrastructure.user.watchlist
{
    /// <summary>Handler for deleting a catalog item from user's watchlist command</summary>
    public class DeleteWatchlistCommandHandler : IAsyncRequestHandler<DeleteWatchlistCommand, Result<Success>>
    {
        private readonly IWatchlistRepository _watchlistRepository;
        
        /// <inheritdoc />
        public DeleteWatchlistCommandHandler (IWatchlistRepository watchlistRepository)
        {
            _watchlistRepository = watchlistRepository;
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(DeleteWatchlistCommand message)
        {
            var result = await _watchlistRepository.DeleteItemAsync(message.UserId.Value, message.Item);

            if (!result.Success)
                return Result<Success>.FromError(result.Error);

            return Result<Success>.FromValue(new Success
            {
                Code = 1,
                Message = "Delete successful"
            });
        }
    }
}