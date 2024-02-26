using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Resturant.MessagesBus;
using Resturant.PaymentProcsser;
using Resturant.Sercices.PaymentApi.Messages;
using Resturant.Sercices.PaymentApi.RabbitMqSender;

namespace Resturant.Sercices.PaymentApi.Messaging
{
    public class RabbitMqPaymentConsumer:BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IRabbitMqPaymentSender _rabbitMqPaymentSenderSender;
        private readonly IProcessorpayment _processorpayment;
        private  IConnection _coonection;
        private IModel _channel;
        private readonly string queueName;
        private readonly string _host;
        private readonly string _password;
        private readonly string _name;

        public RabbitMqPaymentConsumer(IConfiguration configuration, 
            IRabbitMqPaymentSender rabbitMqPaymentSenderSender,IProcessorpayment processorpayment)
        {
            _configuration = configuration;
            _rabbitMqPaymentSenderSender = rabbitMqPaymentSenderSender;
            _processorpayment = processorpayment;
            queueName = _configuration.GetValue<string>("OueueTopicName");
            _host = _configuration.GetValue<string>("RabbitHost");
            _name = _configuration.GetValue<string>("RabbitName");
            _password = _configuration.GetValue<string>("RabbitPassword");
            var factory = new ConnectionFactory()
            {
                HostName = _host,
                UserName = _name,
                Password = _password
            };
            _coonection = factory.CreateConnection();
            _channel = _coonection.CreateModel();
            _channel.QueueDeclare(queueName, false, false, false, arguments: null);

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                PaymentRequest paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequest>(content);
                handleMessage(paymentRequestMessage).GetAwaiter().GetResult();
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(queueName, false, consumer); 
            return Task.CompletedTask;
        }
        private async Task handleMessage(PaymentRequest paymentRequestMessage)
        {
            var result = _processorpayment.PaymentProccess();
            UpdatePaymentResult updatePayment = new()
            {
                OrderId = paymentRequestMessage.OrderId,
                Status = result,
                Email = paymentRequestMessage.Email
            };
            try
            {
               _rabbitMqPaymentSenderSender.SendMessage(updatePayment);
                //await _messageBus.PublishMessage(updatePayment, OrderUpdateTopic);
                //await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
