using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.watchlist;

namespace z5.ms.infrastructure.user.watchlist
{
    /// <summary>Handler for adding a new catalog item to user's watchlist command </summary>
    public class AddWatchlistCommandHandler : IAsyncRequestHandler<AddWatchlistCommand, Result<Success>>
    {
        private readonly IWatchlistRepository _watchlistRepository;
        
        /// <inheritdoc />
        public AddWatchlistCommandHandler(IWatchlistRepository watchlistRepository)
        {
            _watchlistRepository = watchlistRepository;
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(AddWatchlistCommand message)
        {
            var result = await _watchlistRepository.AddItemAsync(message.UserId.Value, message.Item);

            if (!result.Success)
                return Result<Success>.FromError(result.Error);

            return Result<Success>.FromValue(new Success
            {
                Code = 1,
                Message = "Watchlist was added successfully"
            });
        }
    }
}