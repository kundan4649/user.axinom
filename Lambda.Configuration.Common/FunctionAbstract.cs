using System;
using AwsClientLibrary;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using z5.ms.common.abstractions;
using z5.ms.common.AWS;
using z5.ms.common.infrastructure.azure.serverless;
using z5.ms.common.notifications;
//using z5.ms.domain.subscription.payment.events;
//using z5.ms.infrastructure.subscription.payment.functions.adyen;

namespace AWS.Lambda.Configuration.Common
{
    public abstract class FunctionAbstract
    {
        protected readonly IConfiguration ConfigService;

        protected readonly ServiceProvider ServiceProvider;

        protected FunctionAbstract()
        {
            // Set up Dependency Injection.
            var serviceCollection = new ServiceCollection();

            this.ConfigureServices(serviceCollection);
            this.ServiceProvider = serviceCollection.BuildServiceProvider();

            // Get Configuration Service from DI system.
            ConfigService = this.ServiceProvider.GetService<IConfigurationService>().GetConfiguration();
            AWSHelper.ConfigService = ConfigService;
            
        }

        protected void ConfigureServices(IServiceCollection serviceCollection)
        {

            serviceCollection.AddLogging(logging => SetupLogger(EnvironmentName.Development == "Development", logging));

            // Register services with DI system
            serviceCollection.AddTransient<IEnvironmentService, EnvironmentService>();
            serviceCollection.AddTransient<IConfigurationService, ConfigurationService>();
            
            serviceCollection.AddTransient<IAWSPublisher<Notification>, SqsQueuePublisher<Notification>>();
            serviceCollection.AddTransient<IAWSPublisher<NotificationMessageEmail>, SqsQueuePublisher<NotificationMessageEmail>>();
            serviceCollection.AddTransient<IAWSPublisher<NotificationMessageSms>, SqsQueuePublisher<NotificationMessageSms>>();

            serviceCollection.AddTransient<ISqsQueueClient<NotificationMessageEmail>, SqsQueueClient<NotificationMessageEmail>>();
            serviceCollection.AddTransient<ISqsQueueClient<NotificationMessageSms>, SqsQueueClient<NotificationMessageSms>>();
            serviceCollection.AddTransient<ISqsQueueClient<Notification>, SqsQueueClient<Notification>>();

            serviceCollection.AddTransient<IPublisher<Notification>, SqsPublisher<Notification>>();
            serviceCollection.AddTransient<IPublisher<NotificationMessageEmail>, SqsPublisher<NotificationMessageEmail>>();
            serviceCollection.AddTransient<IPublisher<NotificationMessageSms>, SqsPublisher<NotificationMessageSms>>();


        }

        public static void SetupLogger(bool isDevelopment, ILoggingBuilder logging)
        {
            if (logging == null)
            {
                throw new ArgumentNullException(nameof(logging));
            }

            // Create and populate LambdaLoggerOptions object
            var loggerOptions = new LambdaLoggerOptions
            {
                IncludeCategory = true,
                IncludeLogLevel = true,
                IncludeNewline = true,
                IncludeEventId = true,
                IncludeException = true
            };

            // Configure Lambda logging
            logging.AddLambdaLogger(loggerOptions);
        }

    }
}
