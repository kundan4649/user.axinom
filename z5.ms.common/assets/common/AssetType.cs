namespace z5.ms.common.assets.common
{
    /// <summary>The available asset types</summary>
    public enum AssetType : short
    {
        /// <summary>Movie</summary>
        Movie = 0,

        /// <summary>Episode</summary>
        Episode = 1,
        
        /// <summary>Season</summary>
        Season = 2,
        
        /// <summary>Album</summary>
        Album = 3,

        /// <summary>Content Set</summary>
        ContentSet = 4,
        
        /// <summary>Track</summary>
        Track = 5,
        
        /// <summary>TV show</summary>
        TvShow = 6,

        /// <summary>Collection</summary>
        Collection = 8,

        /// <summary>Channel</summary>
        Channel = 9,

        /// <summary>EPG program</summary>
        EpgProgram = 10,
        
        /// <summary>Subscription plan</summary>
        SubscriptionPlan = 11,
        
        /// <summary>Notification</summary>
        Notification = 12,
        
        /// <summary>Promo code</summary>
        PromoCode = 13,

        /// <summary>Live event</summary>
        LiveEvent = 14,

        /// <summary>File</summary>
        File = 15,
        
        /// <summary>Genre List</summary>
        GenreList = 16,

        /// <summary>Unknown asset</summary>
        Unknown = 99,

        /// <summary>Custom</summary>
        Custom = 100,

        /// <summary>ExternalLink</summary>
        ExternalLink = 101
    }
}