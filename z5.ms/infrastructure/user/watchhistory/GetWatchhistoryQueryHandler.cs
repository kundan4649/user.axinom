using System.Collections.Generic;
using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;
using z5.ms.domain.user.watchhistory;

namespace z5.ms.infrastructure.user.watchhistory
{
    /// <summary>Handler for get user's watch history query</summary>
    public class GetWatchhistoryQueryHandler : IAsyncRequestHandler<GetWatchhistoryQuery, Result<List<CatalogItem>>>
    {
        private readonly IWatchHistoryRepository _watchHistoryRepository;

        /// <inheritdoc />
        public GetWatchhistoryQueryHandler(IWatchHistoryRepository watchHistoryRepository)
        {
            _watchHistoryRepository = watchHistoryRepository;
        }

        /// <inheritdoc />
        public async Task<Result<List<CatalogItem>>> Handle(GetWatchhistoryQuery message)
        {
            return await _watchHistoryRepository.GetItemsAsync(message.UserId.Value);
        }
    }
}