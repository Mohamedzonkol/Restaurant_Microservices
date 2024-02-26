using Resturant.Services.Discount.Dtos;

namespace Resturant.Services.Discount.Reposerty
{
    public interface ICopounRepoeserty
    {
        Task<CouponDto> GetCouponByCode(string CouponCode);
    }
}
