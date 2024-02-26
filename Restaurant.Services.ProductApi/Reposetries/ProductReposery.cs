using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant.Services.ProductApi.DbContexts;
using Restaurant.Services.ProductApi.DTO;
using Restaurant.Services.ProductApi.Models;

namespace Restaurant.Services.ProductApi.Reposetries
{
    public class ProductReposery:IProductReposetry
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductReposery(AppDbContext context,IMapper mapper)
        {
            _mapper= mapper;
            _context= context;
        }
        public async Task<IEnumerable<ProdutDto>> GetProducts()
        {
            List<Product> ProductList =await _context.Products.ToListAsync();
            return _mapper.Map<List<ProdutDto>>(ProductList);
        }

        public async Task<ProdutDto> GetProductById(int productId)
        {
            Product product = await _context.Products.Where(x=>x.ProductId== productId).FirstOrDefaultAsync();
            return _mapper.Map<ProdutDto>(product);
        }

        public async Task<ProdutDto> CreateUpdateProduct(ProdutDto productDto)
        {
            Product product =_mapper.Map<ProdutDto, Product>(productDto);
            if (product.ProductId>0)
            {
                _context.Products.Update(product);
            }
            else
            {
                _context.Products.AddAsync(product);
            }
           await _context.SaveChangesAsync();
           return _mapper.Map<Product, ProdutDto>(product);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                Product product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
                if (product is null)
                {
                    return false;
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
