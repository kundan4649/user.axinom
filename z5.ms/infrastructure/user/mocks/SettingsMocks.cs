using System;
using System.Collections.Generic;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.mocks
{
    /// <summary>
    /// Mock data for testing Settings endpoints
    /// </summary>
    public class SettingsMocks
    {
        /// <summary>Mock settings data</summary>
        public List<SettingItem> Settings = new List<SettingItem>
        {
            new SettingItem
            {
                SettingKey = "persistent-playback",
                SettingValue = "0-0-Transformers|0|92|2017-04-20T17:43:31.27Z"
            },
            new SettingItem
            {
                SettingKey = "persistent-playback",
                SettingValue = "0-0-Logan|0|95|2017-01-20T17:43:31.27Z"
            },
            new SettingItem
            {
                SettingKey = "persistent-playback",
                SettingValue = "0-0-GhostInTheShell|0|91|2017-07-20T17:43:31.27Z"
            },
            new SettingItem
            {
                SettingKey = "persistent-playback",
                SettingValue = "0-0-Dangal|0|135|2017-09-20T17:43:31.27Z"
            }
        };

        /// <summary>Mock settings entity data</summary>
        public List<SettingItemEntity> SettingsEntities = new List<SettingItemEntity>
        {
            new SettingItemEntity
            {
                Id = Guid.Parse("3e7bbddb-acf0-4b63-b721-0be1751ed87e"),
                UserId = Guid.Parse("5f1b80d0-b1d6-493d-96ae-41d0d0c34a9e"),
                SettingKey = "persistent-playback",
                SettingValue = "0-0-Transformers|0|92|2017-04-20T17:43:31.27Z"
            },
            new SettingItemEntity
            {
                Id = Guid.Parse("082a6feb-497c-4ff4-8fd1-263c20c9b76c"),
                UserId = Guid.Parse("5f1b80d0-b1d6-493d-96ae-41d0d0c34a9e"),
                SettingKey = "persistent-playback",
                SettingValue = "0-0-Logan|0|95|2017-01-20T17:43:31.27Z"
            },
            new SettingItemEntity
            {
                Id = Guid.Parse("4167db74-61e5-4ce2-aa58-b95eeff63877"),
                UserId = Guid.Parse("5f1b80d0-b1d6-493d-96ae-41d0d0c34a9e"),
                SettingKey = "persistent-playback",
                SettingValue = "0-0-GhostInTheShell|0|91|2017-07-20T17:43:31.27Z"
            },
            new SettingItemEntity
            {
                Id = Guid.Parse("baf745ce-96a3-497d-a54a-5eadbb44fb9d"),
                UserId = Guid.Parse("5f1b80d0-b1d6-493d-96ae-41d0d0c34a9e"),
                SettingKey = "persistent-playback",
                SettingValue = "0-0-Dangal|0|135|2017-09-20T17:43:31.27Z"
            }
        };
    }
}
