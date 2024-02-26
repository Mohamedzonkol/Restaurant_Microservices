using Newtonsoft.Json;
using Resturant.services.Cart.DTO;

namespace Resturant.services.Cart.Reposerty
{
    public class CouponRepoeserty:ICouponRepoeserty
    {
        private readonly HttpClient _client;

        public CouponRepoeserty(HttpClient client)
        {
            _client = client;
        }
        public async Task<CouponDto> GetCoupon(string couponName)
        {
            var response = await _client.GetAsync($"/api/coupon/{couponName}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
            }

            return new CouponDto();
        }
    }
}
