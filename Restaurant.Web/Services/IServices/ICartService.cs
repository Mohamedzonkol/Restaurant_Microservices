using Restaurant.Web.Models;

namespace Restaurant.Web.Services.IServices
{
    public interface ICartService
    {
        Task<T>GetCartByUserID<T>(string Userid,string token=null);
        Task<T>AddToCart<T>(CartDto cart,string token=null);
        Task<T>UpdateCart<T>(CartDto cart,string token=null);
        Task<T>RemoveFromCart<T>(int cartId,string token=null);
        Task<T> ApplyCoupon<T>(CartDto cart, string token = null);
        Task<T> RemoveCoupon<T>(string userID, string token = null);
        Task<T> Checkout<T>(CartHeaderDto cartHeader, string token = null);

    }
}
