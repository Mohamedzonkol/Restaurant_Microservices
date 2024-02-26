using Azure;
using Microsoft.AspNetCore.Mvc;
using Resturant.MessagesBus;
using Resturant.services.Cart.DTO;
using Resturant.services.Cart.Messages;
using Resturant.services.Cart.RabbitMqSender;
using Resturant.services.Cart.Reposerty;


namespace Resturant.services.Cart.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartApiController : Controller
    {
        private readonly ICartReposerty _cartReposerty;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        private readonly ICouponRepoeserty _couponRepoeserty;
        private readonly IRabbitMqCartSender _reMqCartSender;
        protected ResponseDto responseDto;

        public CartApiController(ICartReposerty cartReposerty,IMessageBus messageBus,
            IConfiguration configuration,ICouponRepoeserty couponRepoeserty,IRabbitMqCartSender reMqCartSender)
        {
            _cartReposerty = cartReposerty;
            _messageBus = messageBus;
            _configuration = configuration;
            _couponRepoeserty = couponRepoeserty;
            _reMqCartSender = reMqCartSender;
            this.responseDto= new ResponseDto();
        }
        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = await _cartReposerty.GetCartByUserID(userId);
                responseDto.Result = cartDto;
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMassages = new List<string>() { ex.ToString() };
            }
            return responseDto;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await _cartReposerty.CreateOrUpdateCart(cartDto);
                responseDto.Result = cartDt;
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMassages = new List<string>() { ex.ToString() };
            }
            return responseDto;
        }

        [HttpPost("UpdateCart")]
        public async Task<object> UpdateCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await _cartReposerty.CreateOrUpdateCart(cartDto);
                responseDto.Result = cartDt;
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMassages = new List<string>() { ex.ToString() };
            }
            return responseDto;
        }

        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody] int cartId)
        {
            try
            {
                bool isSuccess = await _cartReposerty.RemoveProductFromCart(cartId);
                responseDto.Result = isSuccess;
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMassages = new List<string>() { ex.ToString() };
            }
            return responseDto;
        }   
        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                bool isSuccess = await _cartReposerty.ApplyCoupon(cartDto.CartHeader.UserId,cartDto.CartHeader.CouponCode);
                responseDto.Result = isSuccess;
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMassages = new List<string>() { ex.ToString() };
            }
            return responseDto;
        }   
        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] string UserId)
        {
            try
            {
                bool isSuccess = await _cartReposerty.RemoveCoupon(UserId);
                responseDto.Result = isSuccess;
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMassages = new List<string>() { ex.ToString() };
            }
            return responseDto;
        }
        [HttpPost("Checkout")]
        public async Task<object> Checkout(CheckoutCardHeaderDto checkoutHeader)
        {
            try
            {
                CartDto cart = await _cartReposerty.GetCartByUserID(checkoutHeader.UserId);
                if (cart is null)
                {
                    return BadRequest();
                }

                if (!string.IsNullOrEmpty(checkoutHeader.CouponCode))
                {
                    CouponDto coupon = await _couponRepoeserty.GetCoupon(checkoutHeader.CouponCode);
                    if (checkoutHeader.DiscountTotal!=coupon.DiscountAmount)
                    {
                        responseDto.IsSuccess=false;
                        responseDto.ErrorMassages = new List<string>() { "Coupon Price IS Change ,Please Refresh" };
                        responseDto.Message = "Coupon Price IS Change ,Please Refresh";
                        return responseDto;
                    }
                }
                checkoutHeader.CartDetail = cart.CartDetails;
                //logic to add message to prosess order
                // string tobicName= "cheackoutmessafetopic";
                //Using Azure Service Bus
               // await _messageBus.PublishMessage(checkoutHeader, "checkoutqueue");
                //USing RabbitMQ
                _reMqCartSender.SendMessage(checkoutHeader, "checkoutqueue");
                 await _cartReposerty.ClearCart(checkoutHeader.UserId);
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
