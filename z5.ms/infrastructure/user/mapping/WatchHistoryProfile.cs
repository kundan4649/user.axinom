using System;
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
    public class WatchHistoryProfile : Profile
    {
        /// <inheritdoc />
        public WatchHistoryProfile()
        {
            // Published WatchHistory DTO to WatchHistory model
            CreateMap<WatchHistoryEntity, CatalogItem>()
                .EqualityComparison((odto, o) => odto.AssetId == o.AssetId)
                .ForMember(dest => dest.Date, opts => opts.MapFrom(src => DateTime.SpecifyKind(src.Date, DateTimeKind.Utc)));

            // Published WatchHistory model to WatchHistory DTO
            CreateMap<CatalogItem, WatchHistoryEntity>()
                .EqualityComparison((odto, o) => odto.AssetId == o.AssetId);

            CreateMap<KeyValuePair<string, WatchHistoryEntity>, KeyValuePair<string, CatalogItem>>()
                .ConstructUsing(x => new KeyValuePair<string, CatalogItem>(x.Key, Mapper.Map<CatalogItem>(x.Value)));
        }
    }
}