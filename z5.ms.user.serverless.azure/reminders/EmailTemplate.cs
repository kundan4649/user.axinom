namespace z5.ms.user.serverless.azure.reminders
{
    /// <summary>Email template to send registration, confirmation etc. emails</summary>
    public class EmailTemplate
    {
        /// <summary>Type of email like registration, confirmation etc. for logging purposes</summary>
        public string Type { get; set; }

        /// <summary>Name to be shown in from name of email</summary>
        public string FromName { get; set; }

        /// <summary>Email to be shown in from name of email</summary>
        public string FromEmail { get; set; }

        /// <summary>Bcc emails if applicable</summary>
        public string BccEmail { get; set; }

        /// <summary>Email subject</summary>
        public string Subject { get; set; }

        /// <summary>Name be shown in reply name of email</summary>
        public string ReplyName { get; set; }

        /// <summary>Email be shown in reply name of email</summary>
        public string ReplyEmail { get; set; }

        /// <summary>Email body</summary>
        public string Body { get; set; }

    }
}
