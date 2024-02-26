using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Resturant.MessagesBus
{
    public class AzureServiceBus:IMessageBus
    {
        private readonly IConfiguration _configuration;
        private string connectionString= "Endpoint=sb://pizzaresturant.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=57fcMSX/y/5t3nsvm+JeBrFrCZwBIUooZ+ASbLbbuX4=";
       // private string topicName= "cheackoutmessafetopic";
    
        public async Task PublishMessage(MessagesBase messages, string TobicName)
        {
          await using var client = new ServiceBusClient(connectionString);
          ServiceBusSender sender = client.CreateSender(TobicName);
          var jsonMessage = JsonConvert.SerializeObject(messages);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };
            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
//public AzureServiceBus(IConfiguration configuration)
//{
//    _configuration = configuration;
//    connectionString = configuration.GetConnectionString("ServiceBusConnectionString");
////    topicName = configuration["AppSettings:CheckoutMessageTopicName"];
//}
//  TobicName = topicName;
