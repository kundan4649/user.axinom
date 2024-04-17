using System.Collections.Generic;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.mapping
{
    // TODO: cover with tests, make sure the mapping profile is valid
    /// <summary>Mapping profile for Automapper</summary>
    /// <remarks>Map DTO to returned BC entities</remarks>
    public class RemindersProfile : Profile
    {
        /// <inheritdoc />
        public RemindersProfile()
        {
            // Published Watchlist DTO to Watchlist model
            CreateMap<ReminderItemEntity, ReminderItem>()
                .EqualityComparison((odto, o) => odto.AssetId == o.AssetId);

            // Published Watchlist model to Watchlist DTO
            CreateMap<ReminderItem, ReminderItemEntity>()
                .EqualityComparison((odto, o) => odto.AssetId == o.AssetId);

            CreateMap<KeyValuePair<string, ReminderItemEntity>, KeyValuePair<string, ReminderItem>>()
                .ConstructUsing(x => new KeyValuePair<string, ReminderItem>(x.Key, Mapper.Map<ReminderItem>(x.Value)));
        }
    }
}