using System;
using Newtonsoft.Json;

namespace z5.ms.domain.entitlement4.viewmodel
{
    /// <summary>Entitlement Message is a JSON data structure designed to instruct the Axinom DRM License Server how to configure a License.</summary>
    public class EntitlementMessage
    {
        /// <summary>The license begin date is the date when the license becomes active.</summary>
        [JsonProperty("begin_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime BeginDate { get; set; }

        /// <summary>The IP address of the device that is allowed to acquire the license.</summary>
        [JsonProperty("client_ip", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ClientIp { get; set; }

        /// <summary>The license expiration date is the date after which the license is not active anymore.</summary>
        [JsonProperty("expiration_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ExpirationDate { get; set; }

        /// <summary>Drm configurations for Fairplay</summary>
        [JsonProperty("fairplay", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Fairplay Fairplay { get; set; }

        /// <summary>Allows to specify for how long the playback is allowed after it has been started for the first time.</summary>
        [JsonProperty("first_play_expiration", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public double? FirstPlayExpiration { get; set; }

        /// <summary>Allows to specify content keys that can be included in the license response.</summary>
        [JsonProperty("keys", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Key[] Keys { get; set; }

        /// <summary>Allows to specify whether keys must be generated based on information from a license request.</summary>
        [JsonProperty("keys_based_on_request", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? KeysBasedOnRequest { get; set; }

        /// <summary>Allows to specify whether the license must be persistent or not.</summary>
        [JsonProperty("persistent", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? Persistent { get; set; }

        /// <summary>Drm configurations for Playready</summary>
        [JsonProperty("playready", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Playready Playready { get; set; }

        /// <summary>Allows to specify whether the client info (additional information about a DRM client) must be returned together with the license response.</summary>
        [JsonProperty("return_client_info", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? ReturnClientInfo { get; set; }

        /// <summary>Type of message</summary>
        [JsonProperty("type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; } = "entitlement_message";

        /// <summary>Drm configurations for Widevine</summary>
        [JsonProperty("widevine", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Widevine Widevine { get; set; }
    }

    /// <summary>Drm configurations for Widevine</summary>
    public class Widevine
    {
        /// <summary>Allows to specify the CGMS-A rule that must be used by the device while playing the protected media.</summary>
        [JsonProperty("cgms-a", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CgmsA { get; set; }

        /// <summary>The ID of the device that is allowed to acquire the license.</summary>
        [JsonProperty("device_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceId { get; set; }

        /// <summary>Allows to specify the security level that the device must have in order to use the license.</summary>
        [JsonProperty("device_security_level", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? DeviceSecurityLevel { get; set; }

        /// <summary>Allows to specify the version of HDCP that must be used in order to play protected media.</summary>
        [JsonProperty("hdcp", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Hdcp { get; set; }
    }

    /// <summary>Drm configurations for Playready</summary>
    public class Playready
    {
        /// <summary>Allows to specify the output protection level for analog video content.</summary>
        [JsonProperty("analog_video_opl", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? AnalogVideoOpl { get; set; }

        /// <summary>Allows to specify output protections that are allowed to play protected analog video content.</summary>
        [JsonProperty("analog_video_output_protections", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public OOutputProtection[] AnalogVideoOutputProtections { get; set; }

        /// <summary>Allows to specify the output protection level for compressed digital audio content.</summary>
        [JsonProperty("compressed_digital_audio_opl", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? CompressedDigitalAudioOpl { get; set; }

        /// <summary>Allows to specify the output protection level for compressed digital video content.</summary>
        [JsonProperty("compressed_digital_video_opl", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? CompressedDigitalVideoOpl { get; set; }

        /// <summary>Allows to specify the custom data of the license response.</summary>
        [JsonProperty("custom_data", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CustomData { get; set; }

        /// <summary>The ID of the device that is allowed to acquire the license.</summary>
        [JsonProperty("device_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceId { get; set; }

        /// <summary>Allows to specify output protections that are allowed to play protected digital audio content.</summary>
        [JsonProperty("digital_audio_output_protections", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public OOutputProtection[] DigitalAudioOutputProtections { get; set; }

        /// <summary>Allows to specify output protections that are allowed to play protected digital video content.</summary>
        [JsonProperty("digital_video_output_protections", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public OOutputProtection[] DigitalVideoOutputProtections { get; set; }

        /// <summary>Allows to specify the minimum application security level that the application must have in order to use the license.</summary>
        [JsonProperty("min_app_security_level", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? MinAppSecurityLevel { get; set; }

        /// <summary>Allows to set the list of IDs of technologies to which protected content is allowed to flow.</summary>
        [JsonProperty("play_enablers", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string[] PlayEnablers { get; set; }

        /// <summary>Allows to specify whether the license must expire in real time, during the playback.</summary>
        [JsonProperty("real_time_expiration", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? RealTimeExpiration { get; set; }

        /// <summary>Allows to specify the identifier of the source content protection system.</summary>
        [JsonProperty("source_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? SourceId { get; set; }

        /// <summary>Allows to specify the output protection level for uncompressed digital audio content.</summary>
        [JsonProperty("uncompressed_digital_audio_opl", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? UncompressedDigitalAudioOpl { get; set; }

        /// <summary>uncompressed_digital_video_opl</summary>
        [JsonProperty("uncompressed_digital_video_opl", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? UncompressedDigitalVideoOpl { get; set; }
    }

    /// <summary>Output protection to specify the content type should be protected</summary>
    public class OOutputProtection
    {
        /// <summary>Any binary data encoded using base64 encoding, which conforms to RFC 4846.</summary>
        [JsonProperty("config_data", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ConfigData { get; set; }

        /// <summary>Oany valid GUID, which represents the ID of the output protection technology.</summary>
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }

    /// <summary>Allows to specify content keys that can be included in the license response.</summary>
    public class Key
    {
        /// <summary>The content key itself (exactly 16 bytes), encrypted using the Advanced Encryption Standard (AES), using the Cipher Block Chaining (CBC) mode, without padding, and encoded using the Base64 encoding, which conforms to RFC 4846.</summary>
        [JsonProperty("encrypted_key", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string EncryptedKey { get; set; }

        /// <summary>The identifier of a content key (also known as "key ID" or "KID"). It can be any valid GUID except the empty GUID</summary>
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>The initialization vector (IV; exactly 16 bytes) that is to be used for the decryption of media together with the provided or generated content key. </summary>
        [JsonProperty("iv", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Iv { get; set; }

        /// <summary>The identifier of a key seed that must be used to generate a content key. It can be any valid GUID</summary>
        [JsonProperty("key_seed_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string KeySeedId { get; set; }
    }

    /// <summary>Drm configurations for Fairplay</summary>
    public class Fairplay
    {
        /// <summary>The ID of the device that is allowed to acquire the license.</summary>
        [JsonProperty("device_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceId { get; set; }

        /// <summary>Allows to specify whether the license must expire in real time, during the playback.</summary>
        [JsonProperty("real_time_expiration", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? RealTimeExpiration { get; set; }
    }

    /// <summary>Axinom DRM License Server Message is a JSON data structure designed to deliver data to and from an Axinom DRM License Server</summary>
    public class LicenseMessage
    {
        /// <summary>The begin_date field indicates the date when the Axinom DRM License Server Message becomes active.</summary>
        [JsonProperty("begin_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime BeginDate { get; set; }

        /// <summary>The com_key_id field indicates the ID of the Communication Key that was used to sign an Axinom DRM License Server Message.</summary>
        [JsonProperty("com_key_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ComKeyId { get; set; }

        /// <summary>The expiration_date field indicates the date when the Axinom DRM License Server Message becomes inactive.</summary>
        [JsonProperty("expiration_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ExpirationDate { get; set; }

        /// <summary>The message field contains an object that represents an inner message defined in the context of Axinom DRM.</summary>
        [JsonProperty("message", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public EntitlementMessage Message { get; set; }

        /// <summary>The version field indicates the version of an Axinom DRM License Server Message.</summary>
        [JsonProperty("version", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int Version { get; set; }
    }
}
