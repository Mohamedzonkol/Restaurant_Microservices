using Resturant.MessagesBus;

namespace Resturant.Services.OrderApi.RabbitMqSender
{
    public interface IRabbitMqOrderSender
    {
        void SendMessage(MessagesBase baseMessage,string queueName);
    }
}
