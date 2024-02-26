namespace Resturant.Services.OrderApi.Models
{
    public class OrderHeader
    {
        public int OrderHeaderId { get; set; }

        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double? OrderCount { get; set; }
        public double? DiscountTotal { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime? OrderTime { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? CardNumber { get; set; }
        public string? CVV { get; set; }
        public string? ExpiryMonthYear { get; set; }
        public int? ItemTotal { get; set; } = 1;
        public List<OrderDetails>? OrderDetail { get; set; }
        public bool PayementStatus { get; set; }
    }
}
