using Newtonsoft.Json;
using RabbitMQ.Client;
using Resturant.MessagesBus;
using System.Text;

namespace Resturant.Sercices.PaymentApi.RabbitMqSender
{
    public class RabbitMqPaymentSender: IRabbitMqPaymentSender
    {
        private readonly IConfiguration _configuration;
        private readonly string _host;
        private readonly string _password;
        private readonly string _name;
        private readonly string ExchangeName;
        private readonly string PaymentUpdateEmailQueue;
        private readonly string PaymentUpdateOrderQueue;
        private IConnection _connection;
        public RabbitMqPaymentSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _host = _configuration.GetValue<string>("RabbitHost");
            _name = _configuration.GetValue<string>("RabbitName");
            _password = _configuration.GetValue<string>("RabbitPassword");
            ExchangeName = _configuration.GetValue<string>("ExchangeName");
            PaymentUpdateEmailQueue = _configuration.GetValue<string>("EmailQueue");
            PaymentUpdateOrderQueue = _configuration.GetValue<string>("OrderQueue");
        }
        public void SendMessage(MessagesBase baseMessage)
        {
            if (ConnectionExits())
            {
                using var channel = _connection.CreateModel();
              //  channel.ExchangeDeclare(ExchangeName,ExchangeType.Fanout,false); //Fanout Exchange
                channel.ExchangeDeclare(ExchangeName,ExchangeType.Direct,false); //Dierct Exchange
                channel.QueueDeclare(PaymentUpdateEmailQueue, false, false, false, null);
                channel.QueueDeclare(PaymentUpdateOrderQueue, false, false, false, null);
               channel.QueueBind(PaymentUpdateEmailQueue,ExchangeName,"EmailPayment");
               channel.QueueBind(PaymentUpdateOrderQueue,ExchangeName,"OrderPayment");
                var message = JsonConvert.SerializeObject(baseMessage);
                var body = Encoding.UTF8.GetBytes(message);
               // channel.BasicPublish(exchange: ExchangeName, "", basicProperties: null, body: body); //bubkish Message for Funout
                channel.BasicPublish(exchange: ExchangeName, "EmailPayment", basicProperties: null, body: body);
                channel.BasicPublish(exchange: ExchangeName, "OrderPayment", basicProperties: null, body: body);

            }
        }
        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _host,
                    UserName = _name,
                    Password = _password
                };
                _connection = factory.CreateConnection();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private bool ConnectionExits() 
        {
            if (_connection != null)
                return true;
            CreateConnection();
            return _connection != null;
        }

    }
}
