using System.Runtime.Serialization;

namespace z5.ms.common.assets.common
{
    /// <summary>Interface for a response model implementing the business type property</summary>
    public interface IBusinessType
    {
        /// <summary>Business type property mapped from licensing based on country</summary>
        BusinessType? BusinessType { get; set; }
    }

    //TODO: rename to something meaningful
    /// <summary>The business type of this item. It could be an advertisement based AVOD type or a premium SVOD item type.</summary>
    public enum BusinessType
    {
        /// <summary>Free access for everyone</summary>
        [EnumMember(Value = "free")]
        Free = 0,
    
        /// <summary>Free access after viewing an ad</summary>
        [EnumMember(Value = "advertisement")]
        Advertisement = 1,
    
        /// <summary>Free access for premium users</summary>
        [EnumMember(Value = "premium")]
        Premium = 2,
    
        /// <summary>Free and downloadable by everyone</summary>
        [EnumMember(Value = "free_downloadable")]
        FreeDownloadable = 3,
    
        /// <summary>Free and downloadable after viewing an ad</summary>
        [EnumMember(Value = "advertisement_downloadable")]
        AdvertisementDownloadable = 4,
    
        /// <summary>Downloadable by premium users</summary>
        [EnumMember(Value = "premium_downloadable")]
        PremiumDownloadable = 5,

        /// <summary>Free access for authenticated users</summary>
        [EnumMember(Value = "free_authenticated")]
        FreeAuthenticated = 6,

        /// <summary>Free access after viewing an ad for authenticated users</summary>
        [EnumMember(Value = "advertisement_authenticated")]
        AdvertisementAuthenticated = 7
    }
}