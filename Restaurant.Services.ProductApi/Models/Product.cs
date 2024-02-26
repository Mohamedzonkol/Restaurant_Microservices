using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Restaurant.Services.ProductApi.Models
{
    public class Product
    {
        [Key] 
        public int ProductId { get; set; }
        [Required] 
        public string Name { get; set; }
        [Range(1, 10000)]
        public double Price { get; set; }
        public string Description { get; set; }
        public string CatagoryName { get; set; }    
        public string ImageUrl { get; set; }
    }
}
