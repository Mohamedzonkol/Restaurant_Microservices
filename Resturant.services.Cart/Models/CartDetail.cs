using System.ComponentModel.DataAnnotations.Schema;

namespace Resturant.services.Cart.Models
{
    public class CartDetail
    {
        public int CartDetailId { get; set; }
        public int CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public virtual CartHeader CartHeader { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int? Count { get; set; }

    }
}
