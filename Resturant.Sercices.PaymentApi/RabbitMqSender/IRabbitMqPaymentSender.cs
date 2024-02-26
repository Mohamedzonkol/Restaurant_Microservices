using Resturant.MessagesBus;

namespace Resturant.Sercices.PaymentApi.RabbitMqSender
{
    public interface IRabbitMqPaymentSender
    {
        void SendMessage(MessagesBase baseMessage);
    }
}
