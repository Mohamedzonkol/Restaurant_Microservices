using System.Runtime.InteropServices;
using Restaurant.Web.Models;
using Restaurant.Web.Services.IServices;

namespace Restaurant.Web.Services
{
    public class ProductService:BaseService,IProductServices
    {
        private readonly IHttpClientFactory _httpClient;
        public ProductService(IHttpClientFactory httpClient) : base(httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<T> GetAllProductsAsync<T>(string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                ApiUrl = SD.ProductAPIBass + "/api/products" ,
                AceessToken =token             });
        }
        public async Task<T> GetProductByIdAsync<T>(int id, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                ApiUrl = SD.ProductAPIBass + "/api/products/" + id,
                AceessToken =token 
            });
        }
        public async Task<T> CreateProductAsync<T>(ProductDto product, string token)
        {
          return  await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = product,
                ApiUrl = SD.ProductAPIBass+"/api/products",
                AceessToken = token
            });
        }
        public async Task<T> UpdateProductAsync<T>(ProductDto product, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                ApiUrl = SD.ProductAPIBass + "/api/products" ,
                Data = product,
                AceessToken = token
            });
        }
        public async Task<T> DeleteProductAsync<T>(int id, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                ApiUrl = SD.ProductAPIBass + "/api/products/"+id,
                AceessToken = token
            });
        }
    }
}
