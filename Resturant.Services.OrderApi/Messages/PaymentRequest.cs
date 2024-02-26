using Resturant.MessagesBus;

namespace Resturant.Services.OrderApi.Messages
{
    public class PaymentRequest:MessagesBase
    {
        public int OrderId { get; set; }
        public string? Name { get; set; }
        public string? CardNumber { get; set; }
        public string? CVV { get; set; }
        public string? ExpiryMonthYear { get; set; }
        public double? OrderCount { get; set; }
        public string? Email { get; set; }

    }
}
