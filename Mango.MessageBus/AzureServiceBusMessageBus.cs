using Azure.Messaging.ServiceBus;
//using Microsoft.Azure.ServiceBus;
//using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.MessageBus
{
    public class AzureServiceBusMessageBus : IMessageBus
    {
        //should be in app settings file
        private string connectionString = "Endpoint=sb://mango-restaurant.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=tsvVpCK7o+UUCtdZUi0q17se2CCdVKMc0894bWmHqig=";
        public async Task PublishMessage(BaseMessage message, string topicName)
        {
            await using var client = new ServiceBusClient(connectionString);
            //ISenderClient senderClient = new TopicClient(connectionString, topicName);
            ServiceBusSender sender = client.CreateSender(topicName);

            var jsonMessage = JsonConvert.SerializeObject(message);

            //var finalMessage = new Message(Encoding.UTF8.GetBytes(jsonMessage))
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                //generic for now
                CorrelationId = Guid.NewGuid().ToString()
            };

            //send to Azure MessageBus
            //await senderClient.SendAsync(finalMessage);
            await sender.SendMessageAsync(finalMessage);
            //await senderClient.CloseAsync();
            await client.DisposeAsync();
        }
    }
}
