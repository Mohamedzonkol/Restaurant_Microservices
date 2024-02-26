using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Resturant.MessagesBus;
using Resturant.PaymentProcsser;
using Resturant.Sercices.PaymentApi.Messages;

namespace Resturant.Sercices.PaymentApi.Messaging
{ public class AzureServiceBusConsumer:IAzureServiceBusConsumer
    {
        private readonly string SubscrabtionPayment;
        private readonly string ServiceBusConnectionString;
        private readonly string OrderPaymentTopic;
        private readonly string OrderUpdateTopic;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        private readonly IProcessorpayment _processorpayment;
        private ServiceBusProcessor orderPaymentProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration,IProcessorpayment processorpayment,IMessageBus messageBus)
        {
            _messageBus = messageBus;
            _configuration = configuration;
            _processorpayment = processorpayment;
            ServiceBusConnectionString= _configuration.GetValue<string>("ServiceBusConnectionString");
           SubscrabtionPayment = _configuration.GetValue<string>("OrderPaymentSubscrabtionName");
           OrderPaymentTopic = _configuration.GetValue<string>("OrderPaymentTopic");
           OrderUpdateTopic = _configuration.GetValue<string>("OrderUpdateTopic");

           var client = new ServiceBusClient(ServiceBusConnectionString);

           orderPaymentProcessor = client.CreateProcessor(OrderPaymentTopic, SubscrabtionPayment);
        }
        public async Task Start()
        {
            orderPaymentProcessor.ProcessMessageAsync += ProcessPayment;
            orderPaymentProcessor.ProcessErrorAsync+= ErrorHandler;
            await orderPaymentProcessor.StartProcessingAsync();
        }public async Task Stop()
        {
            await orderPaymentProcessor.StopProcessingAsync();
            await orderPaymentProcessor.DisposeAsync();
        }
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
        private async Task ProcessPayment(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            PaymentRequest paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequest>(body);
            var result = _processorpayment.PaymentProccess();
            UpdatePaymentResult updatePayment = new()
            {
                OrderId = paymentRequestMessage.OrderId,
                Status = result,
                Email = paymentRequestMessage.Email
            };
            try
            {
                await _messageBus.PublishMessage(updatePayment,OrderUpdateTopic);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
