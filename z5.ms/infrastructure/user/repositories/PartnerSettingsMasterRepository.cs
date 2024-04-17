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
    /// PartnerSettingsMaster repository to make get, add, delete operations
    /// </summary>
    public interface IPartnerSettingsMasterRepository : IBaseRepository<PartnerSettingsMasterItemEntity>
    {
        /// <summary>
        /// Get PartnerSettingsMaster items by Partner Id/Name
        /// </summary>
        /// <returns></returns>
        Task<Result<List<PartnerSettingsMasterItem>>> GetItemsAsync();
    }

    /// <inheritdoc cref="IAccessValidationByIPRepository" />
    public class PartnerSettingsMasterRepository : BaseRepository<PartnerSettingsMasterItemEntity>, IPartnerSettingsMasterRepository
    {
        private readonly IMapper _mapper;
        private readonly UserErrors _errors;
        private readonly UserServiceOptions _options;

        /// <summary>
        /// Constructor method of PartnerSettingsMaster Repository
        /// </summary>
        public PartnerSettingsMasterRepository(
            IMapper mapper,
            IOptions<UserErrors> errors,
            IOptions<DbConnectionOptions> dbOptions
            ) : base(dbOptions.Value.MSDatabaseConnection)
        {
            _mapper = mapper;
            _errors = errors.Value;
        }

        /// <inheritdoc />
        public async Task<Result<List<PartnerSettingsMasterItem>>> GetItemsAsync()
        {
            var result = await Get();

            var item = result.Select(i => _mapper.Map<PartnerSettingsMasterItemEntity, PartnerSettingsMasterItem>(i)).ToList();

            return Result<List<PartnerSettingsMasterItem>>.FromValue(item);
        }
    }
}