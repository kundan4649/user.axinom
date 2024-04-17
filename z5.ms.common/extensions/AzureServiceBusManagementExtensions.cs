using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;

namespace z5.ms.common.extensions
{
    /// <summary>Methods for azure service bus management</summary>
    public static class AzureServiceBusManagementExtensions
    {
        /// <summary>Create a topic on an existing azure service if it does not already exist</summary>
        public static async Task CreateTopicIfNotExists(this ManagementClient client, TopicDescription topicDescription)
        {
            // Using exception handling for flow control as there seems no nice way to test entity existance
            try
            {
                await client.CreateTopicAsync(topicDescription).ConfigureAwait(false);
            }
            catch (MessagingEntityAlreadyExistsException)
            {
                // its ok, we want it to exist
            }
        }

        /// <summary>Create a subscription on an existing azure service bus topic if it does not already exist</summary>
        public static async Task CreateSubscriptionIfNotExists(this ManagementClient client, SubscriptionDescription subscriptionDescription)
        {
            if(await client.SubscriptionExistsAsync(subscriptionDescription.TopicPath, subscriptionDescription.SubscriptionName))
                return;
            
            // Using exception handling for flow control as there seems no nice way to test entity existance
            for (var i = 0; i < 6; i++)
            {
                try
                {
                    await client.CreateSubscriptionAsync(subscriptionDescription).ConfigureAwait(false);
                    break;
                }
                catch (Exception)
                {
                    if (i == 5)
                        throw;
                    await Task.Delay(TimeSpan.FromSeconds(10 + i * 5));
                }
            }
        }

        /// <summary>Delete an existing subscription on an azure service bus topic</summary>
        public static async Task DeleteSubscriptionIfExists(this ManagementClient client, string topicName, string subscriptionName)
        {
            if (!await client.SubscriptionExistsAsync(topicName, subscriptionName))
                return;

            for (var i = 1; i <= 6; i++)
            {
                try
                {
                    await client.DeleteSubscriptionAsync(topicName, subscriptionName).ConfigureAwait(false);
                    break;
                }
                catch (Exception)
                {
                    if (i == 5)
                        throw;
                    await Task.Delay(TimeSpan.FromSeconds(10 + i * 5));
                }
            }

        }
    }
}
