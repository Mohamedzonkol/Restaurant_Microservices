using AutoMapper;
using Resturant.services.Cart.DTO;
using Resturant.services.Cart.Models;

namespace Resturant.services.Cart
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var MappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetail, CartDetailDto>().ReverseMap();
                config.CreateMap<Models.Cart, CartDto>().ReverseMap();
            });
            return MappingConfig;
        }
    }
}
