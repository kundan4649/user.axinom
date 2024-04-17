using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using z5.ms.common.infrastructure.db;
using Microsoft.Extensions.Options;
using z5.ms.common.abstractions;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.repositories
{
    /// <summary>
    /// WatchHistory repository to make get, add, delete operations
    /// </summary>
    public interface IWatchHistoryRepository : IBaseRepository<WatchHistoryEntity>
    {
        /// <summary>
        /// Get watch history items for specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result<List<CatalogItem>>> GetItemsAsync(Guid userId);

        /// <summary>
        /// Get watch history items for specified user filtered by asset id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        Task<Result<CatalogItem>> GetItemsByAssetIdAsync(Guid userId, string assetId);

        /// <summary>
        /// Add new item into specified user's watch history list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Result<bool>> AddItemAsync(Guid userId, CatalogItem item);

        /// <summary>
        /// Delete specified item from user's watch history list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Result<bool>> DeleteItemAsync(Guid userId, CatalogItem item);

        /// <summary>
        /// Update specified item from user's watch history list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Result<bool>> UpdateItemAsync(Guid userId, CatalogItem item);
    }

    /// <inheritdoc cref="IWatchHistoryRepository" />
    public class WatchHistoryRepository : BaseRepository<WatchHistoryEntity>, IWatchHistoryRepository
    {
        private readonly IMapper _mapper;
        private readonly UserErrors _errors;

        /// <summary>
        /// Constructor method of WatchHistory Repository
        /// </summary>
        public WatchHistoryRepository(
            IMapper mapper,
            IOptions<UserErrors> errors,
            IOptions<DbConnectionOptions> dbOptions
            ) : base(dbOptions.Value.WatchHistoryDbConnection)
        {
            _mapper = mapper;
            _errors = errors.Value;
        }

        /// <inheritdoc />
        public async Task<Result<List<CatalogItem>>> GetItemsAsync(Guid userId)
        {
            var result = await GetItemsWhere(nameof(WatchHistoryEntity.UserId), userId);
            var items = result.Select(i => _mapper.Map<WatchHistoryEntity, CatalogItem>(i)).ToList();

            return Result<List<CatalogItem>>.FromValue(items.OrderByDescending(x => x.Date).ToList());
        }

        /// <inheritdoc />
        public async Task<Result<CatalogItem>> GetItemsByAssetIdAsync(Guid userId, string assetId)
        {
            var result = await GetItemsWhere(nameof(WatchHistoryEntity.UserId), userId,
                nameof(WatchHistoryEntity.AssetId), assetId);

            var item = _mapper.Map<WatchHistoryEntity, CatalogItem>(result?.FirstOrDefault());

            return Result<CatalogItem>.FromValue(item);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> AddItemAsync(Guid userId, CatalogItem item)
        {
            var existing = await IsAnyWhere(nameof(WatchHistoryEntity.UserId), userId,
                nameof(WatchHistoryEntity.AssetId), item.AssetId);

            if (existing)
                return Result<bool>.FromError(_errors.WatchHistoryItemExists);

            var entityItem = _mapper.Map<CatalogItem, WatchHistoryEntity>(item, opt => opt.AfterMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
                dest.UserId = userId;
                if (item.Date == DateTime.MinValue)
                    dest.Date = DateTime.UtcNow;
            }));

            var result = await Insert(entityItem);

            return result.Success ? Result<bool>.FromValue(true) : Result<bool>.FromError(_errors.WatchHistoryInsertFailed, 500);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> DeleteItemAsync(Guid userId, CatalogItem item)
        {
            int result;
            try
            {
                result = await DeleteItemsWhere(nameof(WatchHistoryEntity.UserId), userId,
                    nameof(WatchHistoryEntity.AssetId), item.AssetId);
            }
            catch (Exception)
            {
                return Result<bool>.FromError(_errors.WatchHistoryDeleteFailed, 500);
            }

            return result > 0 ? Result<bool>.FromValue(true) : Result<bool>.FromError(_errors.ItemNotFound, 404);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> UpdateItemAsync(Guid userId, CatalogItem item)
        {
            var entity = new WatchHistoryEntity();
            var fieldsToUpdate = new List<string>();

            if (item.Date != DateTime.MinValue)
            {
                fieldsToUpdate.Add(nameof(WatchlistEntity.Date));
                entity.Date = item.Date;
            }

            if (item.Duration != 0)
            {
                fieldsToUpdate.Add(nameof(WatchlistEntity.Duration));
                entity.Duration = item.Duration;
            }

            int result;
            try
            {
                result = await UpdateItemsWhere(entity, fieldsToUpdate.ToArray(),
                    nameof(WatchHistoryEntity.UserId), userId, nameof(WatchHistoryEntity.AssetId), item.AssetId);
            }
            catch (Exception)
            {
                return Result<bool>.FromError(_errors.WatchHistoryUpdateFailed, 500);
            }

            return result > 0 ? Result<bool>.FromValue(true) : Result<bool>.FromError(_errors.ItemNotFound, 404);
        }
    }
}