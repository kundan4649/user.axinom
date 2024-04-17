using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using z5.ms.common.notifications;

namespace z5.ms.notification.aws.lambda.function
{
    /// <summary> Function handler to send email messages to SMTP server </summary>
    public static class EmailFunction
    {
        /// <summary> Handle notification email message </summary>
        public static async Task Handle(NotificationMessage message, NotificationOptions config, ILogger log = null)
        {
            await SendEmail(message.To, message.Subject, message.Body, message.From, config, log);
        }
        private static async Task SendEmail(string email, string subject, string body, string fromAddress, NotificationOptions config, ILogger log)
        {
            var client = ConnectSmtpClient(config);
            try
            {
                if (string.IsNullOrEmpty(email) || !MailboxAddress.TryParse(email, out var emailTo))
                {
                    Console.WriteLine($"SendEmail to {email} FAILED with message: Invalid email");
                    return;
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(config.EmailFromName, fromAddress ?? config.EmailFromAddress));
                message.To.Add(emailTo);
                message.Body = new TextPart("html")
                {
                    Text = body.TrimStart()
                };
                message.Subject = subject;


                var start = DateTime.UtcNow;
                await client.SendAsync(message);
                Console.WriteLine($"It took {(DateTime.UtcNow - start).TotalMilliseconds} milliseconds to send email to {email}");
            }
            catch (Exception ex)
            {
                throw new Exception($"SendEmail to {email} FAILED with message: {ex.Message}");
            }
            finally
            {
                DisposeClient(client);
            }
        }

        private static void DisposeClient(SmtpClient client)
        {
            client?.Disconnect(true);
            client?.Dispose();
        }

        private static SmtpClient ConnectSmtpClient(NotificationOptions options)
        {
            var smtpClient = new SmtpClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => true
            };
            var host = options.MailHost;
            var port = options.MailPort;
            smtpClient.Connect(host, port);

            smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            var username = options.MailUserName;
            var password = options.MailPassword;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                smtpClient.Authenticate(username, password);
            }
            return smtpClient;
        }
    }
}
