using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resturant.services.Cart.Models
{
    public class Product
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }
       // [Required]
        public string Name { get; set; }
        [Range(1, 10000)]
        public double? Price { get; set; }
        public string? Description { get; set; }
        public string? CatagoryName { get; set; }
        public string? ImageUrl { get; set; }
    }
}
