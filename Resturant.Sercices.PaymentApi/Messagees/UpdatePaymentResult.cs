using Resturant.MessagesBus;

namespace Resturant.Sercices.PaymentApi.Messages
{
    public class UpdatePaymentResult:MessagesBase
    {
        public int OrderId { get; set; }
        public bool? Status { get; set; }
        public string? Email { get; set; }
    }
}
