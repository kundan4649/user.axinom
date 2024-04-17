using System;

namespace z5.ms.domain.subscription.entitlement.config
{
    /// <summary>
    /// Configuration options for Axinom DRM
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class AxinomDrmOptions
    {
        /// <summary>Axinom Drm service communication key to sign entitlement messages</summary>
        public string AxinomDrmCommunicationKey { get; set; }

        /// <summary>Communication key id indicates the ID of the Communication Key that was used to sign an Axinom DRM License Server Message</summary>
        public string AxinomDrmCommunicationKeyId { get; set; }

        /// <summary>Axinom DRM message options</summary>
        public AxinomDrmMessageOptions DrmMessageOptions { get; set; } = new AxinomDrmMessageOptions();
    }

    /// <summary>Axinom DRM message options</summary>
    [Obsolete("Use entitlement2")]
    public class AxinomDrmMessageOptions
    {
        /// <summary>Drm token expiration hours</summary>
        public int ExpirationHours { get; set; } = 24;

        /// <summary>Drm token expiration hours after first play</summary>
        public int ExpirationAfterFirstPlayHours { get; set; } = 24;

        /// <summary>PlayReady options</summary>
        public AxinomPlayReady PlayReady { get; set; }

        /// <summary>Widevine options</summary>
        public AxinomWidevine Widevine { get; set; }
    }

    /// <summary>PlayReady options</summary>
    [Obsolete("Use entitlement2")]
    public class AxinomPlayReady
    {
        /// <summary>Mininum application ecurity level</summary>
        public int? MinAppSecurityLevel { get; set; }

        /// <summary>Real time expiration enable/disable</summary>
        public bool RealTimeExpiration { get; set; }

        /// <summary>Output protection level for compressed digital audio content</summary>
        public int? CompressedDigitalAudioOpl { get; set; }

        /// <summary>Output protection level for uncompressed digital audio content</summary>
        public int? UncompressedDigitalAudioOpl { get; set; }

        /// <summary>Output protection level for compressed digital video content</summary>
        public int? CompressedDigitalVideoOpl { get; set; }

        /// <summary>Output protection level for uncompressed digital video content</summary>
        public int? UncompressedDigitalVideoOpl { get; set; }

        /// <summary>Output protection level for analog video content</summary>
        public int? AnalogVideoOpl { get; set; }

        /// <summary>List of IDs of technologies to which protected content is allowed to flow</summary>
        public string PlayEnablers { get; set; }
    }

    /// <summary>Widevine options</summary>
    [Obsolete("Use entitlement2")]
    public class AxinomWidevine
    {
        /// <summary>Allows to specify the CGMS-A rule that must be used by the device while playing the protected media.</summary>
        public string CgmsA { get; set; }

        /// <summary>The ID of the device that is allowed to acquire the license.</summary>
        public string DeviceId { get; set; }

        /// <summary>Allows to specify the security level that the device must have in order to use the license.</summary>
        public int? DeviceSecurityLevel { get; set; }

        /// <summary>Allows to specify the version of HDCP that must be used in order to play protected media.</summary>
        public string Hdcp { get; set; }
    }
}