using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Resturant.Services.OrderApi.Messages;
using Resturant.Services.OrderApi.Repoesetry;

namespace Resturant.Services.OrderApi.Messaging
{
    public class RabbitMqPaymenyConsumer:BackgroundService
    {
        private readonly IConfiguration _configuration;
        private  IConnection _coonection;
        private IModel _channel;
        private readonly string ExchangeName;
        private readonly string _host;
        private readonly string _password;
        private readonly string _name;
        private readonly OrderReposetry _orderReposetry;
        private string queueName = "";
        private readonly string PaymentUpdateOrderQueue;

        public RabbitMqPaymenyConsumer(IConfiguration configuration, OrderReposetry orderReposetry)
        {
            _configuration = configuration;
            ExchangeName = _configuration.GetValue<string>("ExchangeName");
            _host = _configuration.GetValue<string>("RabbitHost");
            _name = _configuration.GetValue<string>("RabbitName");
            _password = _configuration.GetValue<string>("RabbitPassword");
            PaymentUpdateOrderQueue = _configuration.GetValue<string>("OrderQueue");
            _orderReposetry = orderReposetry;
            var factory = new ConnectionFactory()
            {
                HostName = _host,
                UserName = _name,
                Password = _password
            };
            _coonection = factory.CreateConnection();
            _channel = _coonection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName,ExchangeType.Direct);
         //   _channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout);
            // queueName = _channel.QueueDeclare().QueueName; //temporary Queue to read Exchange "Fanout"
            _channel.QueueDeclare(PaymentUpdateOrderQueue, false, false, false, null);
            _channel.QueueBind(PaymentUpdateOrderQueue, ExchangeName, "OrderPayment");//Binding Queue
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                UpdatePaymentResult paymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResult>(content);
                handleMessage(paymentResultMessage).GetAwaiter().GetResult();
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(PaymentUpdateOrderQueue, false, consumer); 
            return Task.CompletedTask;
        }
        private async Task handleMessage(UpdatePaymentResult paymentResultMessage)
        {
            try
            {
                await _orderReposetry.UpdateOrderPayementStatus(paymentResultMessage.OrderId, paymentResultMessage.Status);

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
