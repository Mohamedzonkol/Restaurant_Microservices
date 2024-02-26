using System.ComponentModel.DataAnnotations;

namespace Resturant.services.Cart.DTO
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }

        public string? UserId { get; set; }
        public string? CouponCode { get; set; }

    }
}
