using System.Collections.Generic;
using z5.ms.domain.user.viewmodels;
using z5.ms.common.assets.common;

namespace z5.ms.infrastructure.user.mocks
{
    /// <summary>
    /// Mock data for testing Reminders endpoints
    /// </summary>
    public class RemindersMocks
    {
        /// <summary>Mock reminders data</summary>
        public List<ReminderItem> Reminders = new List<ReminderItem>
        {
            new ReminderItem
            {
                AssetId = "0-6-GameOfThrones",
                AssetType = AssetType.TvShow,
                ReminderType = ReminderType.Email
            },
            new ReminderItem
            {
                AssetId = "0-6-Westworld",
                AssetType = AssetType.TvShow,
                ReminderType = ReminderType.Mobile
            },
            new ReminderItem
            {
                AssetId = "0-6-Lucifer",
                AssetType = AssetType.TvShow,
                ReminderType = ReminderType.Email
            },
            new ReminderItem
            {
                AssetId = "0-6-BigBangTheory",
                AssetType = AssetType.TvShow,
                ReminderType = ReminderType.Mobile
            }
        };
    }
}
