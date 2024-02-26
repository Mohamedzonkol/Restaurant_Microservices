using Microsoft.AspNetCore.Mvc;
using Resturant.Services.Discount.Dtos;
using Resturant.Services.Discount.Reposerty;

namespace Resturant.Services.Discount.Controllers
{

    [ApiController]
    [Route("api/coupon")]
    public class CouponController : Controller
    {
        private readonly ICopounRepoeserty _copounRepoeserty;
        protected ResponseDto responseDto;

        public CouponController(ICopounRepoeserty copounRepoeserty)
        {
            _copounRepoeserty = copounRepoeserty;
            this.responseDto = new ResponseDto();
        }
        [HttpGet("{code}")]
        public async Task<object> GetDiscount(string code)
        {
            try
            {
                CouponDto couponDto = await _copounRepoeserty.GetCouponByCode(code);
                responseDto.Result = couponDto;
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMassages = new List<string>() { ex.ToString() };
            }
            return responseDto;
        }
    }
}
