using Microsoft.AspNetCore.Mvc;
using Restaurant.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Restaurant.Web.Services.IServices;
using Restaurant.Web.Services;

namespace Restaurant.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductServices _productServices;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger,IProductServices productServices,ICartService cartService)
        {
            _logger = logger;
            _productServices = productServices;
            _cartService = cartService;
        }

        public async Task<IActionResult >Index()
        {
            List<ProductDto> listProduct = new();
            var result = await _productServices.GetAllProductsAsync<ResponseDto>("");
            if (result!=null&&result.IsSuccess)
            {
                listProduct = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(result.Result));
            }
            return View(listProduct);
        }
        [Authorize]
        public async Task<IActionResult > Details(int productId)
        {
            ProductDto product = new();
            var result = await _productServices.GetProductByIdAsync<ResponseDto>(productId, "");
            if (result != null && result.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(result.Result));
            }
            return View(product);
        }
        [Authorize]
        [HttpPost]
        [ActionName("Details")]
        public async Task<IActionResult > DetailsPost(ProductDto productDto)
        {
            CartDto cartDto = new()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
                }
            };

            CartDetailDto cartDetails = new CartDetailDto()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId
            };

            var resp = await _productServices.GetProductByIdAsync<ResponseDto>(productDto.ProductId, "");
            if (resp != null && resp.IsSuccess)
            {
                cartDetails.Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(resp.Result));
            }
            List<CartDetailDto> cartDetailsDtos = new();
            cartDetailsDtos.Add(cartDetails);
            cartDto.CartDetails = cartDetailsDtos;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var addToCartResp = await _cartService.AddToCart<ResponseDto>(cartDto, accessToken);
            if (addToCartResp != null && addToCartResp.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize]
        public async Task<IActionResult> Login()
        {
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
    
}
