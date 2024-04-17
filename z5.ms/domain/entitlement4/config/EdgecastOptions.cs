namespace z5.ms.domain.entitlement4.config
{
    /// <summary>
    /// Options to configure the Edgecast CDN token provider
    /// </summary>
    public class EdgecastOptions
    {
        /// <summary>The primary key used for encoding claims</summary>
        public string PrimaryKey { get; set; }

        /// <summary>Validity period (in h) of the Edgecast token</summary>
        public int ExpirationTime { get; set; }
    }
}