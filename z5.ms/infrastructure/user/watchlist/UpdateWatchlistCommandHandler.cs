using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.watchlist;

namespace z5.ms.infrastructure.user.watchlist
{
    /// <summary>Handler for updating user's watchlist command</summary>
    public class UpdateWatchlistCommandHandler : IAsyncRequestHandler<UpdateWatchlistCommand, Result<Success>>
    {
        private readonly IWatchlistRepository _watchlistRepository;
        
        /// <inheritdoc />
        public UpdateWatchlistCommandHandler (IWatchlistRepository watchlistRepository)
        {
            _watchlistRepository = watchlistRepository;
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(UpdateWatchlistCommand message)
        {
            var result = await _watchlistRepository.UpdateItemAsync(message.UserId.Value, message.Item);

            if (!result.Success)
                return Result<Success>.FromError(result.Error);

            return Result<Success>.FromValue(new Success
            {
                Code = 1,
                Message = "Watchlist was updated successfully"
            });
        }
    }
}