using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z5.ms.common.abstractions;
using z5.ms.common.infrastructure.db;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.repositories
{
    /// <summary>
    /// AccessValidationByIP repository to make get, add, delete operations
    /// </summary>
    public interface IAccessValidationByIPRepository : IBaseRepository<AccessValidationByIPItemEntity>
    {
        /// <summary>
        /// Get AccessValidationByIP items for specified combination
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="requestEndPoint"></param>
        /// <returns></returns>
        Task<Result<AccessValidationByIPItem>> GetItemAsync(string ipAddress, string requestEndPoint);

        /// <summary>
        /// Add new item into specified AccessValidationByIP list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Result<bool>> AddItemAsync(AccessValidationByIPItem item);

        /// <summary>
        /// Delete specified item from AccessValidationByIP list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<bool>> DeleteItemAsync(Guid id);

        /// <summary>
        /// Update specified item from AccessValidationByIP list
        /// </summary>
        /// <param name="item"></param>
        /// <param name="requestCount"></param>
        /// <returns></returns>
        Task<Result<bool>> UpdateItemAsync(AccessValidationByIPItem item, bool resetCount, bool isABadRequest);
    }

    /// <inheritdoc cref="IAccessValidationByIPRepository" />
    public class AccessValidationByIPRepository : BaseRepository<AccessValidationByIPItemEntity>, IAccessValidationByIPRepository
    {
        private readonly IMapper _mapper;
        private readonly UserErrors _errors;
        private readonly UserServiceOptions _options;

        /// <summary>
        /// Constructor method of Reminders Repository
        /// </summary>
        public AccessValidationByIPRepository(
            IMapper mapper,
            IOptions<UserErrors> errors,
            IOptions<DbConnectionOptions> dbOptions
            ) : base(dbOptions.Value.MSDatabaseConnection)
        {
            _mapper = mapper;
            _errors = errors.Value;
        }

        /// <inheritdoc />
        public async Task<Result<AccessValidationByIPItem>> GetItemAsync(string ipAddress, string requestEndPoint)
        {
            var result = await GetItemsWhere(nameof(AccessValidationByIPItem.IpAddress), ipAddress, nameof(AccessValidationByIPItem.RequestEndPoint), requestEndPoint);
            var item = result.Select(i => _mapper.Map<AccessValidationByIPItemEntity, AccessValidationByIPItem>(i)).ToList().FirstOrDefault();

            return Result<AccessValidationByIPItem>.FromValue(item);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> AddItemAsync(AccessValidationByIPItem item)
        {
            var existing = await IsAnyWhere(nameof(AccessValidationByIPItem.IpAddress), item.IpAddress,
                nameof(AccessValidationByIPItem.RequestEndPoint), item.RequestEndPoint);

            if (existing)
                return Result<bool>.FromError(_errors.RequestURLEntryExistsWithThisIpAddress);

            var entityItem = _mapper.Map<AccessValidationByIPItem, AccessValidationByIPItemEntity>(item, opt => opt.AfterMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
                dest.DateCreated = DateTime.UtcNow;
                dest.RequestCount = 1;
                dest.LastRequestReceivedDate = DateTime.UtcNow;
            }));

            var result = await Insert(entityItem);

            return result.Success ?
                Result<bool>.FromValue(true) :
                Result<bool>.FromError(_errors.AccessValidationByIPAddFailed, 500);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> DeleteItemAsync(Guid id)
        {
            int result;

            try
            {
                result = await DeleteItemsWhere(nameof(AccessValidationByIPItem.Id), id);
            }
            catch (Exception)
            {
                return Result<bool>.FromError(_errors.AccessValidationByIPDeleteFailed, 500);
            }

            return result > 0 ? Result<bool>.FromValue(true) : Result<bool>.FromError(_errors.ItemNotFound, 404);
        }

        /// <inheritdoc />
        public async Task<Result<bool>> UpdateItemAsync(AccessValidationByIPItem item, bool resetCount, bool isABadRequest)
        {
            var entity = new AccessValidationByIPItemEntity();
            var fieldsToUpdate = new List<string> { nameof(entity.RequestCount), nameof(entity.LastRequestReceivedDate), nameof(entity.RequestBlockedDate) };

            entity.RequestCount = resetCount ? 1 : item.RequestCount + 1;
            entity.LastRequestReceivedDate = DateTime.UtcNow;
            entity.RequestBlockedDate = isABadRequest && !item.RequestBlockedDate.HasValue ? DateTime.UtcNow : item.RequestBlockedDate;
            //entity.IsRequestBlocked = !resetCount;

            int result;

            try
            {
                result = await UpdateItemsWhere(entity, fieldsToUpdate.ToArray(),
                    nameof(AccessValidationByIPItem.IpAddress), item.IpAddress,
                    nameof(AccessValidationByIPItem.RequestEndPoint), item.RequestEndPoint);
            }
            catch (Exception ex)
            {
                return Result<bool>.FromError(_errors.AccessValidationByIPUpdateFailed, 500);
            }

            return result > 0 ? Result<bool>.FromValue(true) : Result<bool>.FromError(_errors.ItemNotFound, 404);
        }
    }
}