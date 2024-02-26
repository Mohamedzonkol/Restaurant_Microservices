

using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Restuarnt.Services.Email.Messages;
using Restuarnt.Services.Email.Repoesetry;

namespace Restuarnt.Services.Email.Messaging
{
    public class RabbitMqEmailConsumer:BackgroundService
    {
        private readonly IConfiguration _configuration;
        private  IConnection _coonection;
        private IModel _channel;
        private readonly string ExchangeName;
        private readonly string _host;
        private readonly string _password;
        private readonly string _name;
        private readonly EmailReposetry _EmailReposetry;
        private string queueName = "";
        private readonly string PaymentUpdateEmailQueue;

        public RabbitMqEmailConsumer(IConfiguration configuration, EmailReposetry emailReposetry)
        {
            _configuration = configuration;
            ExchangeName = _configuration.GetValue<string>("ExchangeName");
            _host = _configuration.GetValue<string>("RabbitHost");
            _name = _configuration.GetValue<string>("RabbitName");
            _password = _configuration.GetValue<string>("RabbitPassword");
            PaymentUpdateEmailQueue = _configuration.GetValue<string>("EmailQueue");
            _EmailReposetry =emailReposetry;
            var factory = new ConnectionFactory()
            {
                HostName = _host,
                UserName = _name,
                Password = _password
            };
            _coonection = factory.CreateConnection();
            _channel = _coonection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName,ExchangeType.Direct);
           // queueName = _channel.QueueDeclare().QueueName; //temporary Queue to read Exchange "Fanout"
           _channel.QueueDeclare(PaymentUpdateEmailQueue, false, false, false, null);
            _channel.QueueBind(PaymentUpdateEmailQueue, ExchangeName, "EmailPayment");//Binding Queue

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
            _channel.BasicConsume(PaymentUpdateEmailQueue, false, consumer); 
            return Task.CompletedTask;
        }
        private async Task handleMessage(UpdatePaymentResult paymentResultMessage)
        {
            try
            {
                await _EmailReposetry.SendAndEmailLog(paymentResultMessage);

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
