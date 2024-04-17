namespace z5.ms.common.notifications
{
    /// <summary>Base implementation for template defining a notification</summary>
    public abstract class NotificationTemplate
    {
        /// <summary>Type of email like registration, confirmation etc. for logging purposes</summary>
        public string TemplateName { get; set; }

        /// <summary>Email subject</summary>
        public string Subject { get; set; }

        /// <summary>Message body</summary>
        public string Body { get; set; }
    }


    /// <summary>Template defining an email notification</summary>
    public class EmailTemplate : NotificationTemplate
    {

    }

    /// <summary>Template defining an sms notification</summary>
    public class SmsTemplate : NotificationTemplate
    {
    }
}
