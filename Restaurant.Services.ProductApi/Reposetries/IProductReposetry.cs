using Restaurant.Services.ProductApi.DTO;

namespace Restaurant.Services.ProductApi.Reposetries
{
    public interface IProductReposetry
    {
        Task<IEnumerable<ProdutDto>> GetProducts();
        Task<ProdutDto> GetProductById(int ProductId);
        Task<ProdutDto> CreateUpdateProduct(ProdutDto product);
        Task<bool>DeleteProduct(int ProductId);
    }
}
