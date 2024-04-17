using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.favorites
{
    // TODO: cover with tests, make sure the mapping profile is valid
    /// <summary>Mapping profile for Automapper</summary>
    /// <remarks>Map DTO to returned BC entities</remarks>
    public class FavoritesProfile : Profile
    {
        /// <inheritdoc />
        public FavoritesProfile()
        {
            // Published Favorites DTO to Favorites model
            CreateMap<FavoriteEntity, CatalogItem>()
                .EqualityComparison((odto, o) => odto.AssetId == o.AssetId)
                .ForMember(dest => dest.Date, opts => opts.MapFrom(src => DateTime.SpecifyKind(src.Date, DateTimeKind.Utc)));

            // Published Favorites model to Favorites DTO
            CreateMap<CatalogItem, FavoriteEntity>()
                .EqualityComparison((odto, o) => odto.AssetId == o.AssetId)
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<KeyValuePair<string, FavoriteEntity>, KeyValuePair<string, CatalogItem>>()
                .ConstructUsing(x => new KeyValuePair<string, CatalogItem>(x.Key, Mapper.Map<CatalogItem>(x.Value)));
        }
    }
}