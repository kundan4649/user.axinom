using System;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Amazon.S3;
using Microsoft.Extensions.Logging;
using z5.ms.common.infrastructure.assetcache;

namespace z5.ms.common.notifications
{
    public static class NotificationHelpers
    {
        public static async Task<T> GetXmlTemplate<T>(string storageConnection, string templateName, string rootName = null, ILogger log = null) where T : class
        {
            using (var stream = await AzureStorage.GetFile(storageConnection, "notifications", $"{templateName}.xml"))
            {
                if (stream == null)
                    return null;

                var serializer = rootName == null ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));

                try
                {
                    return (T) serializer.Deserialize(stream);
                }
                catch (Exception e)
                {
                    log?.LogError($"Unable to deserialize XML template. Error: {e.Message}");
                    return null;
                }
            }
        }

        public static async Task<T> GetXmlTemplateFromS3<T>(IAmazonS3 amazonS3Client, string templateBucketName, string templateName, string rootName = null, ILogger log = null) where T : class
        {
            if (string.IsNullOrEmpty(templateBucketName))
                templateBucketName = "notificationacceptance";

            using (var stream = await AWSStorage.GetFile(amazonS3Client, templateBucketName, $"{templateName}.xml"))
            {
                if (stream == null)
                    return null;

                var serializer = rootName == null ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));

                try
                {
                    return (T)serializer.Deserialize(stream);
                }
                catch (Exception e)
                {
                    log?.LogError($"Unable to deserialize XML template. Error: {e.Message}");
                    return null;
                }
            }
        }

    }
}