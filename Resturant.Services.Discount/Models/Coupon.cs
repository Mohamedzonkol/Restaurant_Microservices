using System.ComponentModel.DataAnnotations;

namespace Resturant.Services.Discount.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }

        public string? CopounCode { get; set; }
        public double?DiscountAmount { get; set; }
    }
}
