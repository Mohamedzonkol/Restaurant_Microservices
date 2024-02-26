using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Resturant.Services.OrderApi.Repoesetry;
using System.Threading.Channels;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using Resturant.Services.OrderApi.Messages;
using Resturant.Services.OrderApi.Models;
using Resturant.Services.OrderApi.RabbitMqSender;

namespace Resturant.Services.OrderApi.Messaging
{
    public class RabbitMqConsumer:BackgroundService
    {
        private readonly OrderReposetry _orderReposetry;
        private readonly IConfiguration _configuration;
        private readonly IRabbitMqOrderSender _rabbitMqOrderSender;
        private  IConnection _coonection;
        private IModel _channel;
        private readonly string queueName;
        private readonly string _host;
        private readonly string _password;
        private readonly string _name;

        public RabbitMqConsumer(OrderReposetry orderReposetry,IConfiguration configuration,IRabbitMqOrderSender rabbitMqOrderSender)
        {
            _orderReposetry = orderReposetry;
            _configuration = configuration;
            _rabbitMqOrderSender = rabbitMqOrderSender;
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
            _channel.QueueDeclare(queueName, false, false, false, null);

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                CheckoutCardHeaderDto cardHeaderDto = JsonConvert.DeserializeObject<CheckoutCardHeaderDto>(content);
                handleMessage(cardHeaderDto).GetAwaiter().GetResult();
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(queueName, false, consumer); 
            return Task.CompletedTask;
        }
        private async Task handleMessage(CheckoutCardHeaderDto cardHeaderDto)
        {
            OrderHeader orderheader = new()
            {
                UserId = cardHeaderDto.UserId,
                FirstName = cardHeaderDto.FirstName,
                LastName = cardHeaderDto.LastName,
                CardNumber = cardHeaderDto.CardNumber,
                OrderDetail = new List<OrderDetails>(),
                CouponCode = cardHeaderDto.CouponCode,
                DiscountTotal = cardHeaderDto.DiscountTotal,
                Email = cardHeaderDto.Email,
                ExpiryMonthYear = cardHeaderDto.ExpiryMonthYear,
                OrderTime = DateTime.Now,
                OrderCount = cardHeaderDto.OrderCount,
                PayementStatus = false,
                Phone = cardHeaderDto.Phone,
                DateTime = cardHeaderDto.DateTime,
                CVV = cardHeaderDto.CVV
            };
            foreach (var detaildList in cardHeaderDto.CartDetail)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = detaildList.ProductId,
                    ProductName = detaildList.Product.Name,
                    Price = detaildList.Product.Price,
                    Count = detaildList.Count
                };
                orderheader.ItemTotal += detaildList.Count;
                orderheader.OrderDetail.Add(orderDetails);
            }

            await _orderReposetry.AddOrder(orderheader);
            PaymentRequest paymentRequest = new()
            {
                Name = orderheader.FirstName + " " + orderheader.LastName,
                CVV = orderheader.CVV,
                CardNumber = orderheader.CardNumber,
                ExpiryMonthYear = orderheader.ExpiryMonthYear,
                OrderId = orderheader.OrderHeaderId,
                OrderCount = orderheader.OrderCount,
                Email = orderheader.Email
            };
            try
            {
                _rabbitMqOrderSender.SendMessage(paymentRequest,"orderpaymenttopic");
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
