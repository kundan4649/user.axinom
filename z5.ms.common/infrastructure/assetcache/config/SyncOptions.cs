using System;
using System.Collections.Generic;

namespace z5.ms.common.infrastructure.assetcache.config
{
    /// <summary>Settings for sync and assets cache functionality</summary>
    public class SyncOptions
    {
        /// <summary>signature character of auxiliary and temporary files</summary>
        public string AssetNameTempChar => "_"; //hard coded

        /// <summary>Blob Storage connection string. Should be used by default</summary>
        public string PublishStorageConnection { get; set; }

        /// <summary>Blob Storage connection string for resource files. Should be used by default</summary>
        public string ResourceStorageConnection { get; set; }

        /// <summary>
        /// Additional Blob storage connection string. Don't use directly, use RandomStorageConnection instead.
        /// </summary>
        public string PublishStorageConnection2 { get; set; }
        
        /// <summary>Container name in the blob storage where the published assets are stored</summary>
        public string PublishContainerName { get; set; }

        /// <summary>Container name in the blob storage where the resource files are stored</summary>
        public string ResourceContainerName { get; set; }

        /// <summary>Service bus connection string</summary>
        public string CmStoMsConnection { get; set; }

        /// <summary>Topic name under which publish messages are pushed</summary>
        public string PublishTopicName { get; set; }

        /// <summary>Name of the publish service bus </summary>
        public string PublishBusName { get; set; }

        /// <summary>
        /// Start using random storage connection, will remember used connectionString. Use this method to balance
        /// storage fetching load in intensive operations for example inital asset loading.
        /// </summary>
        public string RandomStorageConnection
        {
            get
            {
                if (!string.IsNullOrEmpty(currentStorageConnection))
                    return currentStorageConnection;

                var connections = new List<string>();

                AddIfNotNull(connections, PublishStorageConnection);
                AddIfNotNull(connections, PublishStorageConnection2);

                if (connections.Count == 0) return null;

                return currentStorageConnection = connections[new Random().Next(connections.Count)];
            }
        }

        /// <summary>Return Currently used storage connection Id. -1 if undefined. </summary>
        public int CurrentStorageConnection {
            get
            {
                if (string.IsNullOrEmpty(currentStorageConnection)) return -1;

                if (!string.IsNullOrEmpty(PublishStorageConnection) && currentStorageConnection == PublishStorageConnection)
                    return 0;
                if (!string.IsNullOrEmpty(PublishStorageConnection2) && currentStorageConnection == PublishStorageConnection2)
                    return 1;
                return -1;
            }
        }

        private string currentStorageConnection { get; set; }

        /// <summary> If value is not null, add it to list </summary>
        private void AddIfNotNull(List<string> list, string value)
        {
            if (!string.IsNullOrEmpty(value)) list.Add(value);
        }
    }
}
