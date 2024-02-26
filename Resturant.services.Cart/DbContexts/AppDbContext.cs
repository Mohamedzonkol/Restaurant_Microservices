using Microsoft.EntityFrameworkCore;
using Resturant.services.Cart.Models;

namespace Resturant.services.Cart.DbContexts
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetail>CartDetails { get; set; }

    }
}
