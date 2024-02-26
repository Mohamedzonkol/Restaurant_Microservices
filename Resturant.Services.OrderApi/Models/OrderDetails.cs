using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resturant.Services.OrderApi.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeader CartHeaderDto { get; set; }
        public int ProductId { get; set; }
        public int? Count { get; set; }
        public string? ProductName { get; set; }
        public double? Price { get; set; }
    }
}
