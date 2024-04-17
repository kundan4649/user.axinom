using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using z5.ms.common.infrastructure.db;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.messages;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.repositories
{
    /// <inheritdoc cref="IWatchHistoryRepository" />
    public class WatchHistoryRepositoryAsync : BaseRepository<WatchHistoryEntity>, IWatchHistoryRepository
    {
        private readonly IMapper _mapper;
        private readonly UserErrors _errors;
        private readonly CloudQueue _queue;

        /// <summary>
        /// Constructor method of WatchHistory Repository
        /// </summary>
        public WatchHistoryRepositoryAsync(
            IMapper mapper,
            IOptions<UserErrors> errors,
            IOptions<DbConnectionOptions> dbOptions
            ) : base(dbOptions.Value.WatchHistoryDbConnection)
        {
            _mapper = mapper;
            _errors = errors.Value;
            _queue = InitializeQueue("watchhistory-operations", dbOptions.Value.QueueStorageConnection);
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
            return await SendMessage(userId, item, MessageType.Add);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> DeleteItemAsync(Guid userId, CatalogItem item)
        {
            return await SendMessage(userId, item, MessageType.Delete);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> UpdateItemAsync(Guid userId, CatalogItem item)
        {
            return await SendMessage(userId, item, MessageType.Update);
        }

        private async Task<Result<bool>> SendMessage(Guid userId, CatalogItem item, MessageType type)
        {
            var entityItem = _mapper.Map<CatalogItem, WatchHistoryEntity>(item, opt => opt.AfterMap((src, dest) =>
            {
                dest.UserId = userId;
            }));

            return await SendToQueue(new WatchHistoryMessage
            {
                Type = type,
                Item = entityItem
            });
        }

        private CloudQueue InitializeQueue(string queueName, string queueStorageConnection)
        {
            var queue = CloudStorageAccount.Parse(queueStorageConnection)
                .CreateCloudQueueClient()
                .GetQueueReference(queueName);

            queue.CreateIfNotExistsAsync().Wait();
            return queue;
        }

        private async Task<Result<bool>> SendToQueue(object message)
        {
            try
            {
                var msgString = JsonConvert.SerializeObject(message);
                await _queue.AddMessageAsync(new CloudQueueMessage(msgString));
                return Result<bool>.FromValue(true);
            }
            catch (Exception e)
            {
                return Result<bool>.FromError(1, $"Failed to send message {message} Error: {e.Message}");
            }
        }
    }
}