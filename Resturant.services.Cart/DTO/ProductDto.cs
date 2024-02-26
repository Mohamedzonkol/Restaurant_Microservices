using System.ComponentModel.DataAnnotations;

namespace Resturant.services.Cart.DTO
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public string? Description { get; set; }
        public string? CatagoryName { get; set; }
        public string? ImageUrl { get; set; }
    }
}
