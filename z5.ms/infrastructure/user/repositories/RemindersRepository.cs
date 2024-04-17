using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using z5.ms.common.infrastructure.db;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common;
using z5.ms.common.abstractions;
using z5.ms.common.assets;
using z5.ms.common.assets.common;
using z5.ms.common.helpers;
using z5.ms.common.infrastructure.db;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.repositories
{
    /// <summary>
    /// Reminders repository to make get, add, delete operations
    /// </summary>
    public interface IRemindersRepository : IBaseRepository<ReminderItemEntity>
    {
        /// <summary>
        /// Get reminder items for specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result<List<ReminderItem>>> GetItemsAsync(Guid userId);

        /// <summary>
        /// Add new item into specified user's reminders list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        Task<Result<bool>> AddItemAsync(Guid userId, ReminderItem item, string country);

        /// <summary>
        /// Delete specified item from user's reminders list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Result<bool>> DeleteItemAsync(Guid userId, ReminderItem item);

        /// <summary>
        /// Update specified item from user's reminders list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        Task<Result<bool>> UpdateItemAsync(Guid userId, ReminderItem item, string country);
    }

    /// <inheritdoc cref="IRemindersRepository" />
    public class RemindersRepository : BaseRepository<ReminderItemEntity>, IRemindersRepository
    {
        private readonly IMapper _mapper;
        private readonly UserErrors _errors;
        private readonly UserServiceOptions _options;

        /// <summary>
        /// Constructor method of Reminders Repository
        /// </summary>
        public RemindersRepository(
            IMapper mapper,
            IOptions<UserErrors> errors,
            IOptions<UserServiceOptions> options,
            IOptions<DbConnectionOptions> dbOptions
            ) : base(dbOptions.Value.MSDatabaseConnection)
        {
            _mapper = mapper;
            _errors = errors.Value;
            _options = options.Value;
        }

        /// <inheritdoc />
        public async Task<Result<List<ReminderItem>>> GetItemsAsync(Guid userId)
        {
            var result = await GetItemsWhere(nameof(ReminderItemEntity.UserId), userId);
            var items = result.Select(i => _mapper.Map<ReminderItemEntity, ReminderItem>(i)).ToList();

            return Result<List<ReminderItem>>.FromValue(items);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> AddItemAsync(Guid userId, ReminderItem item, string country)
        {
            if (item.AssetId.GetAssetTypeOrDefault() == (int)AssetType.Episode)
            {
                var episode = await GetAsset<Episode>(item.AssetId, country);
                if (!episode.Success)
                    return Result<bool>.FromError(episode.Error, 404);
                item.AssetId = episode.Value.TvShow.Id;
                item.AssetType = AssetType.TvShow;
            }

            var existing = await IsAnyWhere(nameof(ReminderItemEntity.UserId), userId,
                nameof(ReminderItemEntity.AssetId), item.AssetId);

            if (existing)
                return Result<bool>.FromError(_errors.ReminderExists);

            var entityItem = _mapper.Map<ReminderItem, ReminderItemEntity>(item, opt => opt.AfterMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
                dest.UserId = userId;
            }));

            var result = await Insert(entityItem);

            return result.Success ? Result<bool>.FromValue(true) : Result<bool>.FromError(_errors.ReminderAddFailed);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> DeleteItemAsync(Guid userId, ReminderItem item)
        {
            int result;
            try
            {
                result = await DeleteItemsWhere(nameof(ReminderItemEntity.UserId), userId,
                    nameof(ReminderItemEntity.AssetId), item.AssetId);
            }
            catch (Exception)
            {
                return Result<bool>.FromError(_errors.ReminderDeleteFailed, 500);
            }

            return result > 0 ? Result<bool>.FromValue(true) : Result<bool>.FromError(_errors.ItemNotFound, 404);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> UpdateItemAsync(Guid userId, ReminderItem item, string country)
        {
            var entity = new ReminderItemEntity();
            var fieldsToUpdate = new List<string>();
            entity.ReminderType = item.ReminderType;
            fieldsToUpdate.Add(nameof(item.ReminderType));

            if (item.AssetId.GetAssetTypeOrDefault() == (int)AssetType.Episode)
            {
                var episode = await GetAsset<Episode>(item.AssetId, country);
                if (!episode.Success)
                    return Result<bool>.FromError(episode.Error, 404);
                entity.AssetId = episode.Value.TvShow.Id;
                entity.AssetType = AssetType.TvShow;

                fieldsToUpdate.Add(nameof(item.AssetId));
                fieldsToUpdate.Add(nameof(item.AssetType));
            }

            int result;
            try
            {
                result = await UpdateItemsWhere(entity, fieldsToUpdate.ToArray(),
                    nameof(ReminderItemEntity.UserId), userId,
                    nameof(ReminderItemEntity.AssetId), item.AssetId);
            }
            catch (Exception)
            {
                return Result<bool>.FromError(_errors.ReminderUpdateFailed, 500);
            }

            return result > 0 ? Result<bool>.FromValue(true) : Result<bool>.FromError(_errors.ItemNotFound, 404);
        }

        private async Task<Result<T>> GetAsset<T>(string assetId, string country = null)
        {
            var url = $"{_options.CatalogApiUrl}/{typeof(T).Name.ToLower()}/{assetId}";
            url = string.IsNullOrWhiteSpace(country) ? $"{url}?country={country}" : url;
            var response = await HttpHelpers.JsonHttpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var errorStr = await response.Content.ReadAsStringAsync();
                try
                {
                    var error = JsonConvert.DeserializeObject<Error>(errorStr);
                    return Result<T>.FromError(error, (int)response.StatusCode);
                }
                catch
                {
                    throw new Exception(errorStr);
                }
            }

            var result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            return Result<T>.FromValue(result);
        }
    }
}