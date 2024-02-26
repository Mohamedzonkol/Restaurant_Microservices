using System.IO.Compression;
using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Restuarnt.Services.Email.Messages;
using Restuarnt.Services.Email.Models;
using Restuarnt.Services.Email.Repoesetry;

namespace Restuarnt.Services.Email.Messaging
{ public class AzureServiceBusConsumer:IAzureServiceBusConsumer
    {
        private readonly string SubscrabtionEmail;
        private readonly string ServiceBusConnectionString;
        private readonly string UpdateOrderPaymentTopic;
        private readonly EmailReposetry _EmailReposetry;
        private readonly IConfiguration _configuration;
        private ServiceBusProcessor OrderUpdatePaymentStatusProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration,EmailReposetry EmailReposetry)
        {
            _EmailReposetry = EmailReposetry;
            _configuration = configuration;
           ServiceBusConnectionString= _configuration.GetValue<string>("ServiceBusConnectionString");
           SubscrabtionEmail = _configuration.GetValue<string>("SubsecrebtionName");
           UpdateOrderPaymentTopic = _configuration.GetValue<string>("OrderUpdatePaymentTopic");
           var client = new ServiceBusClient(ServiceBusConnectionString);
           OrderUpdatePaymentStatusProcessor = client.CreateProcessor(UpdateOrderPaymentTopic, SubscrabtionEmail);
        }
        public async Task Start()
        {
            OrderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnPaymenntOrderUpdateMessageRecive;
            OrderUpdatePaymentStatusProcessor.ProcessErrorAsync+= ErrorHandler;
            await OrderUpdatePaymentStatusProcessor.StartProcessingAsync();
        }
        public async Task Stop()
        {
            await OrderUpdatePaymentStatusProcessor.StopProcessingAsync();
            await OrderUpdatePaymentStatusProcessor.DisposeAsync();
        }
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
        private async Task OnPaymenntOrderUpdateMessageRecive(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            UpdatePaymentResult paymentMessage = JsonConvert.DeserializeObject<UpdatePaymentResult>(body);
            try
            {
                await _EmailReposetry.SendAndEmailLog(paymentMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
      
        }
    }
}
