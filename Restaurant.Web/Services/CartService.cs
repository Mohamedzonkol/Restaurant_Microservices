using Restaurant.Web.Models;
using Restaurant.Web.Services.IServices;

namespace Restaurant.Web.Services
{
    public class CartService:BaseService,ICartService
    {
        private readonly IHttpClientFactory _httpClient;
        public CartService(IHttpClientFactory httpClient) : base(httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<T> GetCartByUserID<T>(string UserId, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                ApiUrl = SD.ShoppingCartAPIBass + "/api/cart/GetCart/" + UserId,
                AceessToken = token
            });
        }
        public async Task<T> AddToCart<T>(CartDto cartDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                ApiUrl = SD.ShoppingCartAPIBass + "/api/cart/AddCart",
                AceessToken = token
            });
        }
        public async Task<T> UpdateCart<T>(CartDto cart, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cart,
                ApiUrl = SD.ShoppingCartAPIBass + "/api/cart/UpdateCart",
                AceessToken = token
            });
        }
        public async Task<T> RemoveFromCart<T>(int cartId, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartId,
                ApiUrl = SD.ShoppingCartAPIBass + "/api/cart/RemoveCart",
                AceessToken = token
            });
        }

        public async Task<T> ApplyCoupon<T>(CartDto cart, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cart,
                ApiUrl = SD.ShoppingCartAPIBass + "/api/cart/ApplyCoupon",
                AceessToken = token
            });
        }
        public async Task<T> Checkout<T>(CartHeaderDto cartHeader, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartHeader,
                ApiUrl = SD.ShoppingCartAPIBass + "/api/cart/Checkout",
                AceessToken = token
            });
        }
        public async Task<T> RemoveCoupon<T>(string userID, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = userID,
                ApiUrl = SD.ShoppingCartAPIBass + "/api/cart/RemoveCoupon",
                AceessToken = token
            });
        }

        
    }
}
