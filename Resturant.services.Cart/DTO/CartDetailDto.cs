using Resturant.services.Cart.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Resturant.services.Cart.DTO;

namespace Resturant.services.Cart.DTO
{
    public class CartDetailDto
    {
        public int CartDetailId { get; set; }
        public int CartHeaderId { get; set; }
        public   CartHeaderDto? CartHeaderDto { get; set; }
        public int ProductId { get; set; }
        public  ProductDto? Product { get; set; }
        public int? Count { get; set; }

    }
}
