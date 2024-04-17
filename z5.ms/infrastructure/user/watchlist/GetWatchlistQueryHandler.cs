using System.Collections.Generic;
using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;
using z5.ms.domain.user.watchlist;

namespace z5.ms.infrastructure.user.watchlist
{
    /// <summary>Handler for get user's watchlist query</summary>
    public class GetWatchlistQueryHandler : IAsyncRequestHandler<GetWatchlistQuery, Result<List<CatalogItem>>>
    {
        private readonly IWatchlistRepository _watchlistRepository;
        
        /// <inheritdoc />
        public GetWatchlistQueryHandler (IWatchlistRepository watchlistRepository)
        {
            _watchlistRepository = watchlistRepository;
        }
        
        /// <inheritdoc />
        public async Task<Result<List<CatalogItem>>> Handle(GetWatchlistQuery message)
        {
            var result = await _watchlistRepository.GetItemsAsync(message.UserId.Value);

            if (!result.Success)
                return Result<List<CatalogItem>>.FromError(result.Error);

            return Result<List<CatalogItem>>.FromValue(result.Value);
        }
    }
}