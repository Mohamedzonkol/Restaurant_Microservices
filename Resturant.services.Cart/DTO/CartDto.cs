using Resturant.services.Cart.DTO;
using Resturant.services.Cart.DTO;
using Resturant.services.Cart.Models;

namespace Resturant.services.Cart.DTO
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailDto>?CartDetails { get; set; }
    }
}
