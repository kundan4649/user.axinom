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
    /// Favorites repository to make get, add, delete operations
    /// </summary>
    // TODO Replace result<bool> with result<Success>
    public interface IFavoritesRepository : IBaseRepository<FavoriteEntity>
    {
        /// <summary>
        /// Get favorite items for specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result<List<CatalogItem>>> GetItemsAsync(Guid userId);

        /// <summary>
        /// Add new item into specified user's favorites list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Result<bool>> AddItemAsync(Guid userId, CatalogItem item);

        /// <summary>
        /// Delete specified item from user's favorites list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Result<bool>> DeleteItemAsync(Guid userId, CatalogItem item);

        /// <summary>
        /// Update specified item from user's favorites list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Result<bool>> UpdateItemAsync(Guid userId, CatalogItem item);
    }

    /// <inheritdoc cref="IFavoritesRepository" />
    public class FavoritesRepository : BaseRepository<FavoriteEntity>, IFavoritesRepository
    {
        private readonly IMapper _mapper;
        private readonly UserErrors _errors;

        /// <summary>
        /// Constructor method of Favorites Repository
        /// </summary>
        public FavoritesRepository(
            IMapper mapper,
            IOptions<UserErrors> errors,
            IOptions<DbConnectionOptions> dbOptions
        ) : base(dbOptions.Value.MSDatabaseConnection)
        {
            _mapper = mapper;
            _errors = errors.Value;
        }

        /// <inheritdoc />
        public async Task<Result<List<CatalogItem>>> GetItemsAsync(Guid userId)
        {
            var result = await GetItemsWhere(nameof(FavoriteEntity.UserId), userId);
            var items = result.Select(i => _mapper.Map<FavoriteEntity, CatalogItem>(i)).ToList();

            return Result<List<CatalogItem>>.FromValue(items);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> AddItemAsync(Guid userId, CatalogItem item)
        {
            var existing = await IsAnyWhere(nameof(FavoriteEntity.UserId), userId,
                nameof(FavoriteEntity.AssetId), item.AssetId);

            if (existing)
                return Result<bool>.FromError(_errors.FavouriteExists);

            var entityItem = _mapper.Map<CatalogItem, FavoriteEntity>(item, opt => opt.AfterMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
                dest.UserId = userId;
                if (item.Date == DateTime.MinValue)
                    dest.Date = DateTime.UtcNow;
            }));

            var result = await Insert(entityItem);
            return result.Success ?
                Result<bool>.FromValue(true) :
                Result<bool>.FromError(_errors.FavouriteAddFailed, 500);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> DeleteItemAsync(Guid userId, CatalogItem item)
        {
            int result;
            try
            {
                result = await DeleteItemsWhere(nameof(FavoriteEntity.UserId), userId,
                    nameof(FavoriteEntity.AssetId), item.AssetId);
            }
            catch (Exception)
            {
                return Result<bool>.FromError(_errors.FavouriteDeleteFailed, 500);
            }

            return result > 0 ?
                Result<bool>.FromValue(true) :
                Result<bool>.FromError(_errors.ItemNotFound, 404);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> UpdateItemAsync(Guid userId, CatalogItem item)
        {

            var entity = new FavoriteEntity();
            var fieldsToUpdate = new List<string>();

            if (item.Date != DateTime.MinValue)
            {
                fieldsToUpdate.Add(nameof(FavoriteEntity.Date));
                entity.Date = item.Date;
            }

            if (item.Duration != 0)
            {
                fieldsToUpdate.Add(nameof(FavoriteEntity.Duration));
                entity.Duration = item.Duration;
            }

            int result;
            try
            {
                result = await UpdateItemsWhere(entity, fieldsToUpdate.ToArray(),
                    nameof(FavoriteEntity.UserId), userId, nameof(FavoriteEntity.AssetId), item.AssetId);
            }
            catch (Exception)
            {
                return Result<bool>.FromError(_errors.FavouriteUpdateFailed, 500);
            }

            return result > 0 ? Result<bool>.FromValue(true) : Result<bool>.FromError(_errors.ItemNotFound, 404);
        }
    }
}