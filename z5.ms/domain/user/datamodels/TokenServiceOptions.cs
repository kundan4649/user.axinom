namespace z5.ms.domain.user.datamodels
{
    /// <summary>Configuration for identity server</summary>
    public class TokenServiceOptions
    {
        /// <summary>Life time of the regular access token</summary>
        public int ClientTokenLifeTime { get; set; }

        /// <summary>Secret key for regular access token</summary>
        public string ClientTokenSecret { get; set; } = "secret";

        /// <summary>Life time of the access token with refresh token</summary>
        public int RefreshClientTokenLifeTime { get; set; }

        /// <summary>Secret key for access token with refresh token</summary>
        public string RefreshClientTokenSecret { get; set; } = "secret";
    }
}
