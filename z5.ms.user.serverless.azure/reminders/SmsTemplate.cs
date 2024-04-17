namespace z5.ms.user.serverless.azure.reminders
{
    /// <summary>Sms template to send registration, confirmation etc.</summary>
    public class SmsTemplate
    {
        /// <summary>Type of sms like registration, confirmation etc. for logging purposes</summary>
        public string Type { get; set; }

        /// <summary>Sms body</summary>
        public string Body { get; set; }
    }
}
