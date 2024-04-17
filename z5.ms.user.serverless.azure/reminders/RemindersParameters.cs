using System.ComponentModel.DataAnnotations;
using z5.ms.common.notifications;

namespace z5.ms.user.serverless.azure.reminders
{
    public class RemindersParameters
    {
        /// <summary>Connection to the MS user service internal database</summary>
        [Required]
        public string UserDatabaseConnection { get; set; }

        /// <summary>Uri to an instance of the MS catalog api so that we can query subscription plans</summary>
        [Required]
        public string CatalogApiUrl { get; set; }

        /// <summary>Notification settings</summary>
        public NotificationOptions NotificationOptions { get; set; }
    }
}