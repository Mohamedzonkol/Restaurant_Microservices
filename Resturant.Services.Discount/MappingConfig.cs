using AutoMapper;
using Resturant.Services.Discount.Dtos;
using Resturant.Services.Discount.Models;

namespace Resturant.Services.Discount
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var MappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto,Coupon>().ReverseMap();
                //config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                //config.CreateMap<CartDetail, CartDetailDto>().ReverseMap();
                //config.CreateMap<Models.Cart, CartDto>().ReverseMap();
            });
            return MappingConfig;
        }
    }
}
