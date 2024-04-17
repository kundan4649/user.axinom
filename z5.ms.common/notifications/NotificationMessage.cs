namespace z5.ms.common.notifications
{
    /// <summary> Notification message model </summary>
    public class NotificationMessage
    {
        /// <summary> Email or phone which message will be sent </summary>
        public string To { get; set; }

        /// <summary> Email address which email will be sent from </summary>
        public string From { get; set; }

        /// <summary> Subject of the email </summary>
        public string Subject { get; set; }

        /// <summary> Message body </summary>
        public string Body { get; set; }
    }
}
