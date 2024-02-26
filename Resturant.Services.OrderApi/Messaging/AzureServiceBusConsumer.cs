using System.IO.Compression;
using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Resturant.MessagesBus;
using Resturant.Services.OrderApi.Messages;
using Resturant.Services.OrderApi.Models;
using Resturant.Services.OrderApi.Repoesetry;

namespace Resturant.Services.OrderApi.Messaging
{ public class AzureServiceBusConsumer:IAzureServiceBusConsumer
    {
        private readonly string messageTopic;
        private readonly string SubscrabtionNameCheckout;
        private readonly string ServiceBusConnectionString;
        private readonly string OrderPaymentTopic;
        private readonly string UpdateOrderPaymentTopic;
        private readonly OrderReposetry _orderReposetry;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        private ServiceBusProcessor CheckOutProcessor;
        private ServiceBusProcessor OrderUpdatePaymentStatusProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration,OrderReposetry orderReposetry,IMessageBus messageBus)
        {
            _orderReposetry = orderReposetry;
            _messageBus = messageBus;
            _configuration = configuration;
           ServiceBusConnectionString= _configuration.GetValue<string>("ServiceBusConnectionString");
           messageTopic= _configuration.GetValue<string>("CheckoutMessageTopic");
           SubscrabtionNameCheckout = _configuration.GetValue<string>("SubscrabtionName");
           OrderPaymentTopic = _configuration.GetValue<string>("OrderPaymentTopic");
           UpdateOrderPaymentTopic = _configuration.GetValue<string>("OrderUpdatePaymentTopic");
           var client = new ServiceBusClient(ServiceBusConnectionString);
          // CheckOutProcessor = client.CreateProcessor(messageTopic, SubscrabtionNameCheckout);
          CheckOutProcessor = client.CreateProcessor(messageTopic);//one par override this Queue Name
           OrderUpdatePaymentStatusProcessor = client.CreateProcessor(UpdateOrderPaymentTopic, SubscrabtionNameCheckout);
           //Subscration the same because i Create subsecration in new topic in the same name other yopic
        }
        public async Task Start()
        {
            CheckOutProcessor.ProcessMessageAsync += OnCheackOutMessageRecive;
            CheckOutProcessor.ProcessErrorAsync+= ErrorHandler;
            await CheckOutProcessor.StartProcessingAsync(); 
            
            OrderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnPaymenntOrderUpdateMessageRecive;
            OrderUpdatePaymentStatusProcessor.ProcessErrorAsync+= ErrorHandler;
            await OrderUpdatePaymentStatusProcessor.StartProcessingAsync();
        }
        public async Task Stop()
        {
            await CheckOutProcessor.StopProcessingAsync();
            await CheckOutProcessor.DisposeAsync();
            
            
            await OrderUpdatePaymentStatusProcessor.StopProcessingAsync();
            await OrderUpdatePaymentStatusProcessor.DisposeAsync();
        }
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
        private async Task OnCheackOutMessageRecive(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            CheckoutCardHeaderDto cardHeaderDto = JsonConvert.DeserializeObject<CheckoutCardHeaderDto>(body);
            OrderHeader orderheader = new()
            {
                UserId = cardHeaderDto.UserId,
                FirstName = cardHeaderDto.FirstName,
                LastName = cardHeaderDto.LastName,
                CardNumber = cardHeaderDto.CardNumber,
                OrderDetail = new List<OrderDetails>(),
              //  ItemTotal = cardHeaderDto.ItemTotal,
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
                await _messageBus.PublishMessage(paymentRequest, OrderPaymentTopic);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task OnPaymenntOrderUpdateMessageRecive(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            UpdatePaymentResult paymentResult = JsonConvert.DeserializeObject<UpdatePaymentResult>(body);
            await _orderReposetry.UpdateOrderPayementStatus(paymentResult.OrderId,paymentResult.Status);
            await args.CompleteMessageAsync(args.Message); 
        }
    }
}
