using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using z5.ms.common.abstractions;
using z5.ms.common.assets;
using z5.ms.common.helpers;
using Notification = z5.ms.common.notifications.Notification;

namespace z5.ms.user.serverless.azure.reminders
{
    public static class SendRemindersFunction
    {
        /// <summary>
        /// Handler for serverless function to send reminders based on a schedule
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="queue"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static async Task<Result<Success>> Handle(RemindersParameters parameters, IPublisher<Notification> queue, ILogger log)
        {
            var connection = new SqlConnection(parameters.UserDatabaseConnection);

            log?.LogInformation($"Send reminders function started");

            var epgResult = await GetEpgList(parameters.CatalogApiUrl);
            if (!epgResult.Success)
                return Result<Success>.FromError(epgResult.Error);

            var remindersResult = await GetReminders(connection, epgResult.Value);
            if (!remindersResult.Success)
                return Result<Success>.FromError(remindersResult.Error);

            var emailCount = remindersResult.Value.Count(a => a.Reminder.ReminderType == ReminderType.Email);
            var smsCount = remindersResult.Value.Count(a => a.Reminder.ReminderType == ReminderType.Mobile);

            log?.LogInformation($"There are {emailCount} emails and {smsCount} SMSs to be sent");

            var successSms = 0;
            var successEmail = 0;

            var sender = new ReminderSender(queue, parameters.NotificationOptions);
            foreach (var reminder in remindersResult.Value)
            {
                switch (reminder.Reminder.ReminderType)
                {
                    case ReminderType.Email:
                        if (string.IsNullOrWhiteSpace(reminder.User?.Email))
                        {
                            log.LogInformation($"Missing email for user ID: {reminder.User?.Id}");
                            continue;
                        }
                        sender.SendEpgReminderEmail(reminder);
                        successEmail++;
                        break;

                    case ReminderType.Mobile:

                        if (string.IsNullOrWhiteSpace(reminder.User?.Mobile))
                        {
                            log.LogInformation($"Missing mobile number for user ID: {reminder.User?.Id}");
                            continue;
                        }
                        sender.SendEpgReminderSms(reminder);
                        successSms++;
                        break;
                }
            }

            log?.LogInformation($"{successEmail}/{emailCount} emails and {successSms}/{smsCount} SMSs have been queued successfully.");

            return new Result<Success>();
        }

        /// <summary>
        /// Get Epg list from the catalog api
        /// </summary>
        private static async Task<Result<List<EpgProgramReminder>>> GetEpgList(string catalogApiUrl)
        {
            var uri = new Uri($"{catalogApiUrl}/v1/epg/soon");
            var response = await HttpHelpers.HttpClient.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode
                ? Result<List<EpgProgramReminder>>.FromJson(content)
                : Result<List<EpgProgramReminder>>.FromError(1, content);
        }

        /// <summary>
        /// Get reminders from user database joined provided with epg items
        /// </summary>
        private static async Task<Result<List<EpgReminder>>> GetReminders(IDbConnection connection, List<EpgProgramReminder> epgItems)
        {
            var result = await connection.QueryAsync<UserEntity, ReminderItemEntity, (UserEntity u, ReminderItemEntity r)>(@"
                                SELECT u.Email, u.Mobile, u.FirstName, u.LastName, u.RegistrationCountry, r.* 
                                FROM Users u
                                JOIN Reminders r ON u.Id = r.UserId
                                WHERE r.AssetId IN @AssetIds 
                                AND (u.Email NOT LIKE '%deleted%' OR u.Email IS NULL) AND (u.Mobile NOT LIKE '%deleted%' OR u.Mobile IS NULL)", 
                (u, r) => (u, r),
                new { AssetIds = epgItems.Select(b => b.VodId) });

            var joinedList = from a in result
                             join c in epgItems.Distinct() on a.r.AssetId equals c.VodId
                             select new EpgReminder { Reminder = a.r, Epg = c, User = a.u };

            return Result<List<EpgReminder>>.FromValue(joinedList.ToList());
        }
    }

    /// <summary>Database entity type for User</summary>
    // TODO: When this moves to user domain, use the user entity from there
    [Table("Users")]
    public class UserEntity
    {
        /// <summary>The unique ID of the user</summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>The email address of the user (if available)</summary>
        public string Email { get; set; }

        /// <summary>The mobile phone number of the user (if available)</summary>
        public string Mobile { get; set; }

        /// <summary>The first name of the user</summary>
        public string FirstName { get; set; }

        /// <summary>The last name of the user</summary>
        public string LastName { get; set; }

        /// <summary>Registration country of the user</summary>
        public string RegistrationCountry { get; set; }
    }
}