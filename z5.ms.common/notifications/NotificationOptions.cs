using System.ComponentModel.DataAnnotations;

namespace z5.ms.common.notifications
{
    /// <summary>Configuration settings defining notifications</summary>
    public class NotificationOptions
    {
        private string _frontEndUrl;

        /// <summary>Sms service host url</summary>
        public string SmsHost { get; set; }

        /// <summary>Mail server host name</summary>
        [Required]
        public string MailHost { get; set; }

        /// <summary>Mail server port</summary>
        [Required]
        public int MailPort { get; set; }

        /// <summary>Mail user name to send notification mails</summary>
        [Required]
        public string MailUserName { get; set; }

        /// <summary>Mail password to send notification mails</summary>
        [Required]
        public string MailPassword { get; set; }

        /// <summary>Name to be shown in from name of email</summary>
        [Required]
        public string EmailFromName { get; set; }

        /// <summary>Email to be shown in from name of email</summary>
        [Required]
        public string EmailFromAddress { get; set; }

        /// <summary>Front end url to be used in emails</summary>
        public string FrontEndUrl
        {
            get => _frontEndUrl;
            set => _frontEndUrl = value?.TrimEnd('/');
        }

        /// <summary>Default support email address to be used in emails</summary>
        public string SupportEmail { get; set; }

        /// <summary>Connection string for the blob storage where templates are located</summary>
        public string NotificationStorageConnection { get; set; }
        
        /// <summary>Storage connection string for the notification azure storage queue</summary>
        public string NotificationQueueConnection { get; set; }

        /// <summary>B2B API for sending all the request for all the notification</summary>
        public string b2bApiforNotification { get; set; }
        public string TemplateS3BucketName { get; set; }
        public string B2BApiSecretToken { get; set; }
    }
}