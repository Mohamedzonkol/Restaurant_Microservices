using System.ComponentModel.DataAnnotations;

namespace Restaurant.Services.ProductApi.DTO
{
    public class ProdutDto
    { public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public string Description { get; set; }
        public string CatagoryName { get; set; }
        public string ImageUrl { get; set; }
    }
}
