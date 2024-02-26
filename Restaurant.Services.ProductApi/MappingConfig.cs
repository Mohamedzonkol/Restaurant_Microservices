using AutoMapper;
using Restaurant.Services.ProductApi.DTO;
using Restaurant.Services.ProductApi.Models;
namespace Restaurant.Services.ProductApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var MappingConfig = new MapperConfiguration(config=>
            {
                config.CreateMap<ProdutDto,Product>();
                config.CreateMap<Product,ProdutDto>();
            });
            return MappingConfig;
        }
    }
}
