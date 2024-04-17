using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using z5.ms.common.assets.common;

namespace z5.ms.common.helpers
{
    /// <summary>Maps asset entities to view models</summary>
    public interface IAssetMapper
    {
        /// <summary>Map an asset entity to a view model based on a given country</summary>
        TDest Map<TDest>(EntityBase entity, Action<object, object> afterMap = null, string country = null);
    }

    /// <summary>Maps properties from an asset entity to an existing view model</summary>
    public interface IAssetPropertyMapper
    {
        /// <summary>Maps properties based on a given country</summary>
        void MapProperties(EntityBase src, object dest, string country);
    }

    /// <inheritdoc />
    public class AssetMapper : IAssetMapper
    {
        private readonly List<IAssetPropertyMapper> _propertyMappers;
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public AssetMapper(IMapper mapper, IEnumerable<IAssetPropertyMapper> propertyMappers)
        {
            _mapper = mapper;
            _propertyMappers = propertyMappers.ToList();
        }

        /// <inheritdoc />
        public TDest Map<TDest>(EntityBase entity, Action<object, object> afterMap = null, string country = null)
            => _mapper.Map<TDest>(entity, opt => opt.AfterMap((src, dest)
                =>
            {
                _propertyMappers.ForEach(x => x.MapProperties(entity, dest, country));
                afterMap?.Invoke(src, dest);
            }));
    }

    /// <summary>Extensions methods for asset-specific AutoMapper operations</summary>
    public static class AssetMappingExtensionMethods
    {
        /// <summary>Extension method to map an asset to a view model using an IAssetMapper Implementation</summary>
        public static TDest As<TDest>(this EntityBase entity, IAssetMapper mapper, string country = null, Action<object, object> afterMap = null)
            => mapper.Map<TDest>(entity, afterMap, country);
    }
}