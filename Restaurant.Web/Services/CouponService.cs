using Restaurant.Web.Models;
using Restaurant.Web.Services.IServices;

namespace Restaurant.Web.Services
{
    public class CouponService:BaseService,ICouponService
    {
        private readonly IHttpClientFactory _httpClient;
        public CouponService(IHttpClientFactory httpClient) : base(httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetCouponAsync<T>(string code, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                ApiUrl = SD.DiscountAPIBass + "/api/coupon/" + code,
                AceessToken = token
            });
        }
    }
}
