namespace Resturant.MessagesBus
{
    public interface IMessageBus
    {
        Task PublishMessage(MessagesBase messages,string TobicName );
    }
}
