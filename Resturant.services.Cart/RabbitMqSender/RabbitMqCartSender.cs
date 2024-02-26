using Newtonsoft.Json;
using RabbitMQ.Client;
using Resturant.MessagesBus;
using System.Text;

namespace Resturant.services.Cart.RabbitMqSender
{
    public class RabbitMqCartSender: IRabbitMqCartSender
    {
        private readonly IConfiguration _configuration;
        private readonly string _host;
        private readonly string _password;
        private readonly string _name;
        private IConnection _connection;
        public RabbitMqCartSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _host = _configuration.GetValue<string>("RabbitHost");
            _name = _configuration.GetValue<string>("RabbitName");
            _password = _configuration.GetValue<string>("RabbitPassword");
        }
        public void SendMessage(MessagesBase baseMessage, string queueName)
        {
            if (ConnectionExits())
            {
                using var channel = _connection.CreateModel();
                channel.QueueDeclare(queueName, false, false, false, null);
                var message = JsonConvert.SerializeObject(baseMessage);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
        }
        private  void CreateConnection()
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
        private bool ConnectionExits()//I need to create connection onne time and use it 
        {
            if (_connection!=null)
                return true;
            CreateConnection();
            return _connection!=null;
        }
    }
}
