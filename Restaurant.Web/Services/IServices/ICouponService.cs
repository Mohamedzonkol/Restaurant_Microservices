namespace Restaurant.Web.Services.IServices
{
    public interface ICouponService
    {
        Task<T> GetCouponAsync<T>(string Couponcode,string token);
    }
}
