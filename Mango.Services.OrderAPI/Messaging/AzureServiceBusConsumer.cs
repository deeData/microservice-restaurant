using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumer
    {
        //message passed in from ServiceBus
        private async Task OnCheckOutMessageReceived(ProcessMessageEventArgs args) 
        {
            var message = args.Message;


        }
    }
}
