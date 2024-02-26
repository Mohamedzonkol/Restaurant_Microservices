using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Restaurant.Web.Models;
using Restaurant.Web.Services.IServices;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace Restaurant.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;
        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> products = new ();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var respons = await _productServices.GetAllProductsAsync<ResponseDto>(accessToken);
            if (respons!=null&&respons.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(respons.Result));
            }
            return View(products);
        } 
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var respons = await _productServices.CreateProductAsync<ResponseDto>(model, accessToken);
                if (respons != null && respons.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(model);
        }
        public async Task<IActionResult> Edit(int productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var respons = await _productServices.GetProductByIdAsync<ResponseDto>(productId, accessToken);
            if (respons != null && respons.IsSuccess)
            {
               ProductDto product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(respons.Result));
                return View(product);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var respons = await _productServices.UpdateProductAsync<ResponseDto>(model, accessToken);
                if (respons != null && respons.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var respons = await _productServices.GetProductByIdAsync<ResponseDto>(productId, accessToken);
            if (respons != null && respons.IsSuccess)
            {
               ProductDto product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(respons.Result));
                return View(product);
            }
            return NotFound();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductDto model)
        {

            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var respons = await _productServices.DeleteProductAsync<ResponseDto>(model.ProductId,accessToken);
                if (respons.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }
    }
}
