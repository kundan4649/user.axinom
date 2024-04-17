using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using z5.ms.common.abstractions;
using z5.ms.common.infrastructure.db;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.repositories
{
    public class WatchHistoryRepoWrapper : BaseRepository<WatchHistoryEntity>, IWatchHistoryRepository
    {
        private readonly WatchHistoryRepository _whRepo;
        private readonly WatchHistoryRepositoryAsync _whaRepo;
        private readonly WatchHistoryOptions _whOptions;

        public WatchHistoryRepoWrapper(
            WatchHistoryRepository whRepo,
            WatchHistoryRepositoryAsync whaRepo,
            IOptions<WatchHistoryOptions> whOptions,
            IOptions<DbConnectionOptions> dbOptions
        ) : base(dbOptions.Value.MSDatabaseConnection)
        {
            _whRepo = whRepo;
            _whaRepo = whaRepo;
            _whOptions = whOptions.Value;
        }

        public async Task<Result<List<CatalogItem>>> GetItemsAsync(Guid userId)
        {
            return await _whRepo.GetItemsAsync(userId);
        }

        public async Task<Result<CatalogItem>> GetItemsByAssetIdAsync(Guid userId, string assetId)
        {
            return await _whRepo.GetItemsByAssetIdAsync(userId, assetId);
        }

        public async Task<Result<bool>> AddItemAsync(Guid userId, CatalogItem item)
        {
            return _whOptions.AsyncAdd 
                ? await _whaRepo.AddItemAsync(userId, item) 
                : await _whRepo.AddItemAsync(userId, item);
        }

        public async Task<Result<bool>> DeleteItemAsync(Guid userId, CatalogItem item)
        {
            return _whOptions.AsyncDelete
                ? await _whaRepo.DeleteItemAsync(userId, item)
                : await _whRepo.DeleteItemAsync(userId, item);
        }

        public async Task<Result<bool>> UpdateItemAsync(Guid userId, CatalogItem item)
        {
            return _whOptions.AsyncUpdate
                ? await _whaRepo.UpdateItemAsync(userId, item)
                : await _whRepo.UpdateItemAsync(userId, item);
        }
    }
}
