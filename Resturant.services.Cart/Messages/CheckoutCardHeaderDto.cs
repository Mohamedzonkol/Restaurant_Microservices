using Resturant.MessagesBus;
using Resturant.services.Cart.DTO;

namespace Resturant.services.Cart.Messages
{
    public class CheckoutCardHeaderDto:MessagesBase
    {
        public int? CartHeaderId { get; set; }

        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double? OrderCount { get; set; }
        public double? DiscountTotal { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateTime { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? CardNumber { get; set; }
        public string? CVV { get; set; }
        public string? ExpiryMonthYear { get; set; }
        public int? ItemTotal { get; set; } = 1;
        public IEnumerable<CartDetailDto>?CartDetail { get; set; }
    }
}
