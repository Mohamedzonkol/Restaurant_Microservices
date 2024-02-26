using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Web.Models;
using Restaurant.Web.Services;
using Restaurant.Web.Services.IServices;

namespace Restaurant.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;

        public CartController(IProductServices productServices,ICartService cartService,ICouponService couponService)
        {
            _productServices = productServices;
            _cartService = cartService;
            _couponService = couponService;
        }
        public async Task<IActionResult >CartIndex()
        {
            return View(await LoadCartDtoByUSerId());
        }
        [HttpPost]
        [ActionName("ApplyCoupon")]
        public async Task<IActionResult > ApplyCoupon(CartDto cart)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.ApplyCoupon<ResponseDto>(cart, token);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return  View();
        }
        [HttpPost]
        [ActionName("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon(CartDto cart)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveCoupon<ResponseDto>(cart.CartHeader.UserId, token);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }
        public async Task<IActionResult > Remove(int CartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveFromCart<ResponseDto>(CartDetailsId, token);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoByUSerId());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _cartService.Checkout<ResponseDto>(cartDto.CartHeader, accessToken);
               
                if (!response.IsSuccess)
                {
                    TempData["Error"] = response.Message;
                    return RedirectToAction(nameof(Checkout));
                }
                return RedirectToAction(nameof(Confirmation));
            }
            catch (Exception e)
            {
                return View(cartDto);
            }
        }
        public async Task<IActionResult> Confirmation()
        {
            return View();
        }
        private async Task<CartDto> LoadCartDtoByUSerId()
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.GetCartByUserID<ResponseDto>(userId, token);
            CartDto cart = new();
            if (response!=null&&response.IsSuccess)
            {
                cart = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            }

            if (cart.CartHeader != null)
            {
                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCouponAsync<ResponseDto>(cart.CartHeader.CouponCode, token);
                    if (coupon != null && coupon.IsSuccess)
                    {
                        var couponModel = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(coupon.Result));
                        cart.CartHeader.DiscountTotal = couponModel.DiscountAmount;
                    }
                }
                foreach (var detail in cart.CartDetails)
                {
                    cart.CartHeader.OrderCount += (detail.Product.Price * detail.Count); 
                }

                cart.CartHeader.OrderCount -= cart.CartHeader.DiscountTotal;
            }

            return cart;
        }


    }
}
