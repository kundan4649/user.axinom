using System;

namespace z5.ms.domain.subscription.entitlement.config
{
    /// <summary>
    /// Configuration options for BuyDRM
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class BuyDrmOptions
    {
        /// <summary>File path of the private key file that provided by BuyDrm</summary>
        public string BuyDrmCertificateFilePath { get; set; }

        /// <summary>BuyDrm DRM message options</summary>
        public BuyDrmMessageOptions DrmMessageOptions { get; set; } = new BuyDrmMessageOptions();
    }

    /// <summary>BuyDrm DRM message options</summary>
    [Obsolete("Use entitlement2")]
    public class BuyDrmMessageOptions
    {
        /// <summary>Drm token expiration hours</summary>
        public int ExpirationHours { get; set; } = 24;

        /// <summary>Drm token expiration hours after first play</summary>
        public int ExpirationAfterFirstPlayHours { get; set; } = 24;

        /// <summary>Mininum application ecurity level</summary>
        public int MinAppSecurityLevel { get; set; } = 2000;

        /// <summary>PlayReady options</summary>
        public BuyDrmPlayReady PlayReady { get; set; } = new BuyDrmPlayReady();

        /// <summary>Widevine options</summary>
        public BuyDrmWidevine Widevine { get; set; } = new BuyDrmWidevine();

        /// <summary>FairPlay options</summary>
        public BuyDrmFairPlay FairPlay { get; set; } = new BuyDrmFairPlay();
    }

    /// <summary>PlayReady options</summary>
    [Obsolete("Use entitlement2")]
    public class BuyDrmPlayReady
    {
        /// <summary>Output protection level for compressed digital audio content</summary>
        public int CompressedDigitalAudioOpl { get; set; } = 300;

        /// <summary>Output protection level for uncompressed digital audio content</summary>
        public int UncompressedDigitalAudioOpl { get; set; } = 300;

        /// <summary>Output protection level for compressed digital video content</summary>
        public int CompressedDigitalVideoOpl { get; set; } = 500;

        /// <summary>Output protection level for uncompressed digital video content</summary>
        public int UncompressedDigitalVideoOpl { get; set; } = 300;

        /// <summary>Output protection level for analog video content</summary>
        public int AnalogVideoOpl { get; set; } = 200;

        /// <summary>List of IDs of technologies to which protected content is allowed to flow</summary>
        public string PlayEnablers { get; set; } =
            "786627D8-C2A6-44BE-8F88-08AE255B01A7|" +
            "D685030B-0F4F-43A6-BBAD-356F1EA0049A|" +
            "002F9772-38A0-43E5-9F79-0F6361DCC62A";
    }

    /// <summary>Widevine options</summary>
    [Obsolete("Use entitlement2")]
    public class BuyDrmWidevine
    {
        /// <summary>Specifies whether playback of content is allowed</summary>
        public bool CanPlay { get; set; } = true;

        /// <summary>Persistent policy which defines how long (in hours)</summary>
        public int LicenseDurationHours { get; set; } = 24;

        /// <summary>Specifies how long (in hours) content can be played in total</summary>
        public int PlaybackDurationHours { get; set; } = 24;
    }

    /// <summary>FairPlay options</summary>
    [Obsolete("Use entitlement2")]
    public class BuyDrmFairPlay
    {
                /// <summary>Persistent policy which defines how long (in hours)</summary>
        public int PersistenceHours { get; set; } = 24;
    }
}