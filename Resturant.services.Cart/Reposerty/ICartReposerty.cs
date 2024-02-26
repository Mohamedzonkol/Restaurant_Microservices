using Resturant.services.Cart.DTO;

namespace Resturant.services.Cart.Reposerty
{
    public interface ICartReposerty
    {
        Task<CartDto> GetCartByUserID(string userId);
        Task<CartDto> CreateOrUpdateCart(CartDto cart);
        Task<bool> RemoveProductFromCart(int cartDetailsId);
        Task<bool> ApplyCoupon(string UserID,string CouponCode);
        Task<bool> RemoveCoupon(string UserID);
        Task<bool> ClearCart(string UserId);

    }
}
