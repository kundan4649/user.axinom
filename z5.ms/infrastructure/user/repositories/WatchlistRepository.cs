using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using z5.ms.common.abstractions;
using z5.ms.common.infrastructure.db;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.repositories
{
    /// <summary>
    /// Watchlist repository to make get, add, delete operations
    /// </summary>
    // TODO replace Result<bool> with Result<Success>
    public interface IWatchlistRepository : IBaseRepository<WatchlistEntity>
    {
        /// <summary>
        /// Get watchlist items for specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result<List<CatalogItem>>> GetItemsAsync(Guid userId);

        /// <summary>
        /// Add new item into specified user's watchlist
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Result<bool>> AddItemAsync(Guid userId, CatalogItem item);

        /// <summary>
        /// Delete specified item from user's watchlist
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Result<bool>> DeleteItemAsync(Guid userId, CatalogItem item);

        /// <summary>
        /// Update specified item from user's watchlist
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Result<bool>> UpdateItemAsync(Guid userId, CatalogItem item);
    }

    /// <inheritdoc cref="IWatchlistRepository" />
    public class WatchlistRepository : BaseRepository<WatchlistEntity>, IWatchlistRepository
    {
        private readonly IMapper _mapper;
        private readonly UserErrors _errors;

        /// <summary>
        /// Constructor method of Watchlist Repository
        /// </summary>
        public WatchlistRepository(
            IMapper mapper,
            IOptions<UserErrors> errors,
            IOptions<DbConnectionOptions> dbOptions
            ) : base(dbOptions.Value.MSDatabaseConnection,dbOptions.Value.ReplicaDatabaseConnection)
        {
            _mapper = mapper;
            _errors = errors.Value;
        }

        /// <inheritdoc />
        public async Task<Result<List<CatalogItem>>> GetItemsAsync(Guid userId)
        {
            var result = await GetWatchListItemsWhere(nameof(WatchlistEntity.UserId), userId);
            var items = result.Select(i => _mapper.Map<WatchlistEntity, CatalogItem>(i)).ToList();

            return Result<List<CatalogItem>>.FromValue(items);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> AddItemAsync(Guid userId, CatalogItem item)
        {
            var existing = await IsAnyWhere(nameof(WatchlistEntity.UserId), userId,
                nameof(WatchlistEntity.AssetId), item.AssetId);

            if (existing)
                return Result<bool>.FromError(_errors.WatchListItemExists);

            var entityItem = _mapper.Map<CatalogItem, WatchlistEntity>(item, opt => opt.AfterMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
                dest.UserId = userId;
                if (item.Date == DateTime.MinValue)
                    dest.Date = DateTime.UtcNow;
            }));

            var result = await Insert(entityItem);

            return result.Success ?
                Result<bool>.FromValue(true) :
                Result<bool>.FromError(_errors.WatchListInsertFailed, 500);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> DeleteItemAsync(Guid userId, CatalogItem item)
        {
            int result;
            try
            {
                result = await DeleteItemsWhere(nameof(WatchlistEntity.UserId), userId,
                    nameof(WatchlistEntity.AssetId), item.AssetId);
            }
            catch (Exception)
            {
                return Result<bool>.FromError(_errors.WatchListDeleteFailed, 500);
            }

            return result > 0 ?
                Result<bool>.FromValue(true) :
                Result<bool>.FromError(_errors.ItemNotFound, 404);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> UpdateItemAsync(Guid userId, CatalogItem item)
        {
            var entity = new WatchlistEntity();
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
                    nameof(WatchlistEntity.UserId), userId, nameof(WatchlistEntity.AssetId), item.AssetId);
            }
            catch (Exception)
            {
                return Result<bool>.FromError(_errors.WatchListUpdateFailed, 500);
            }

            return result > 0 ? Result<bool>.FromValue(true) : Result<bool>.FromError(_errors.ItemNotFound, 404);
        }
    }
}
