using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using z5.ms.common.infrastructure.db;
using Microsoft.Extensions.Options;
using z5.ms.common.abstractions;
using z5.ms.common.infrastructure.db;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.repositories
{
    /// <summary>
    /// Settings repository to make get, add, delete operations
    /// </summary>
    // TODO Replace result<bool> with result<Success>
    public interface ISettingsRepository : IBaseRepository<SettingItemEntity>
    {
        /// <summary>
        /// Get all settins of specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Settings</returns>
        Task<Result<List<SettingItem>>> GetItemsAsync(Guid userId);

        /// <summary>
        /// Add new item into specified user's Settings
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Result<bool>> AddItemAsync(Guid userId, SettingItem item);

        /// <summary>
        /// Delete specified item from user's Settings
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Result<bool>> DeleteItemAsync(Guid userId, SettingItem item);

        /// <summary>
        /// Update specified item from user's Settings
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Result<bool>> UpdateItemAsync(Guid userId, SettingItem item);
    }

    /// <inheritdoc cref="ISettingsRepository" />
    public class SettingsRepository : BaseRepository<SettingItemEntity>, ISettingsRepository
    {
        private readonly IMapper _mapper;
        private readonly UserErrors _errors;
        readonly IPartnerSettingsMasterRepository _partnerSettingsMasterRepository;

        /// <summary>
        /// Constructor method of Settings Repository
        /// </summary>
        public SettingsRepository(
            IMapper mapper,
            IOptions<UserErrors> errors,
            IOptions<DbConnectionOptions> dbOptions,
            IPartnerSettingsMasterRepository partnerSettingsMasterRepository
            ) : base(dbOptions.Value.MSDatabaseConnection,dbOptions.Value.ReplicaDatabaseConnection)
        {
            _mapper = mapper;
            _errors = errors.Value;
            _partnerSettingsMasterRepository = partnerSettingsMasterRepository;
        }

        /// <inheritdoc />
        public async Task<Result<List<SettingItem>>> GetItemsAsync(Guid userId)
        {
            var result = await GetSettingItemsWhere(nameof(SettingItemEntity.UserId), userId);
            
            var partnerJsonList = (await _partnerSettingsMasterRepository.GetItemsAsync()).Value;
            
            var items = result.Select(i => _mapper.Map<SettingItemEntity, SettingItem>(i)).Select(i =>
            i.SettingKey.ToLower() == "partner" &&
            partnerJsonList.Select(j => j.PartnerName.ToLower()).Contains(i.SettingValue.ToLower()) ? new SettingItem { SettingKey = i.SettingKey, SettingValue = partnerJsonList.Where(k => k.PartnerName.ToLower() == i.SettingValue.ToLower()).Select(l => l.Json).FirstOrDefault() }
                        : i).ToList();
            return Result<List<SettingItem>>.FromValue(items);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> AddItemAsync(Guid userId, SettingItem item)
        {
            var existing = await IsAnyWhere(nameof(SettingItemEntity.UserId), userId,
                nameof(SettingItemEntity.SettingKey), item.SettingKey);

            if (existing)
                return Result<bool>.FromError(_errors.SettingsExists);

            var entityItem = _mapper.Map<SettingItem, SettingItemEntity>(item, opt => opt.AfterMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
                dest.UserId = userId;
                dest.LastUpdate = DateTime.UtcNow;
            }));

            var result = await Insert(entityItem);

            return result.Success ?
                Result<bool>.FromValue(true) :
                Result<bool>.FromError(_errors.SettingsAddFailed, 500);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> DeleteItemAsync(Guid userId, SettingItem item)
        {
            int result;
            try
            {
                result = await DeleteItemsWhere(nameof(SettingItemEntity.UserId), userId,
                    nameof(SettingItemEntity.SettingKey), item.SettingKey);
            }
            catch (Exception)
            {
                return Result<bool>.FromError(_errors.SettingsDeleteFailed, 500);
            }

            return result > 0 ?
                Result<bool>.FromValue(true) :
                Result<bool>.FromError(_errors.ItemNotFound, 404);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> UpdateItemAsync(Guid userId, SettingItem item)
        {

            var entity = new SettingItemEntity();
            var fieldsToUpdate = new List<string>();


            if (!string.IsNullOrWhiteSpace(item.SettingValue))
            {
                entity.SettingValue = item.SettingValue;
                entity.LastUpdate = DateTime.UtcNow;
                fieldsToUpdate.Add(nameof(SettingItemEntity.SettingValue));
                fieldsToUpdate.Add(nameof(SettingItemEntity.LastUpdate));
            }

            int result;
            try
            {
                result = await UpdateItemsWhere(entity, fieldsToUpdate.ToArray(),
                    nameof(SettingItemEntity.UserId), userId,
                        nameof(SettingItemEntity.SettingKey), item.SettingKey);
            }
            catch (Exception)
            {
                return Result<bool>.FromError(_errors.SettingsUpdateFailed, 500);
            }

            return result > 0 ? Result<bool>.FromValue(true) : Result<bool>.FromError(_errors.ItemNotFound, 404);
        }
    }
}
