namespace Resturant.services.Cart.Models
{
    public class Cart
    {
        public CartHeader CartHeader { get; set; }
        public IEnumerable<CartDetail>CartDetails { get; set; }
    }
}
