using Resturant.MessagesBus;

namespace Resturant.services.Cart.RabbitMqSender
{
    public interface IRabbitMqCartSender
    {
        void SendMessage(MessagesBase baseMessage,string queueName);
    }
}
