using System.Collections.Generic;
using AutoMapper;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.settings
{
    // TODO: cover with tests, make sure the mapping profile is valid
    /// <summary>Mapping profile for Automapper</summary>
    /// <remarks>Map DTO to returned BC entities</remarks>
    public class SettingsProfile : Profile
    {
        /// <inheritdoc />
        public SettingsProfile()
        {
            // Published Favorites DTO to Favorites model
            CreateMap<SettingItemEntity, SettingItem>();

            // Published Favorites model to Favorites DTO
            CreateMap<SettingItem, SettingItemEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdate, opt => opt.Ignore());

            CreateMap<KeyValuePair<string, SettingItemEntity>, KeyValuePair<string, SettingItem>>()
                .ConstructUsing(x => new KeyValuePair<string, SettingItem>(x.Key, Mapper.Map<SettingItem>(x.Value)));
        }
    }
}