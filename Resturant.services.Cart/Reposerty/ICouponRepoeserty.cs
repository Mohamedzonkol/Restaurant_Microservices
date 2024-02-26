using Resturant.services.Cart.DTO;

namespace Resturant.services.Cart.Reposerty
{
    public interface ICouponRepoeserty
    {
        Task<CouponDto> GetCoupon(string couponName);
    }
}
