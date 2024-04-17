using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;
using z5.ms.domain.user.watchhistory;

namespace z5.ms.infrastructure.user.watchhistory
{
    /// <summary>Handler for get user's watch history filtered by asset id query</summary>
    public class GetWatchhistoryByIdQueryHandler : IAsyncRequestHandler<GetWatchhistoryByIdQuery, Result<CatalogItem>>
    {
        private readonly IWatchHistoryRepository _watchHistoryRepository;

        /// <inheritdoc />
        public GetWatchhistoryByIdQueryHandler(IWatchHistoryRepository watchHistoryRepository)
        {
            _watchHistoryRepository = watchHistoryRepository;
        }

        /// <inheritdoc />
        public async Task<Result<CatalogItem>> Handle(GetWatchhistoryByIdQuery message)
        {
            return await _watchHistoryRepository.GetItemsByAssetIdAsync(message.UserId.Value, message.AssetId);
        }
    }
}