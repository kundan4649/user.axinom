using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;
using SimpleMigrations;
using z5.ms.common.extensions;

namespace z5.ms.common.infrastructure.db.migrations
{
    /// <inheritdoc />
    /// <summary> Simple Migration extension for Azure Service Bus </summary>
    public class ServiceBusMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up()
        {
        }

        /// <inheritdoc />
        protected override void Down()
        {
        }

        /// <summary>
        /// Creates a service bus topic if it doesn't exist
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="topicName"></param>
        public async Task CreateServiceBusTopic(string connectionString, string topicName)
        {
            var client = new ManagementClient(connectionString);
            await client.CreateTopicIfNotExists(new TopicDescription(topicName));
        }

        /// <summary>
        /// Creates a service bus subscription if it doesn't exist
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="topicName"></param>
        /// <param name="subscriptionName"></param>
        public async Task CreateServiceBusSubscription(string connectionString, string topicName, string subscriptionName)
        {
            var client = new ManagementClient(connectionString);
            await client.CreateSubscriptionIfNotExists(new SubscriptionDescription(topicName, subscriptionName));
        }

        /// <summary>
        /// Deletes a service bus topic if it exists
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="topicName"></param>
        /// <param name="subscriptionName"></param>
        public async Task DeleteServiceBusSubscription(string connectionString, string topicName, string subscriptionName)
        {
            var client = new ManagementClient(connectionString);
            await client.DeleteSubscriptionIfExists(topicName, subscriptionName);
        }
    }
}
