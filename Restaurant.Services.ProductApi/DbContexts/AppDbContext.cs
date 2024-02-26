using Microsoft.EntityFrameworkCore;
using Restaurant.Services.ProductApi.Models;

namespace Restaurant.Services.ProductApi.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        ///Seeding Data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Pizza",
                Price = 15.99,
                CatagoryName = "Food",
                Description = "Delicious pizza with assorted toppings",
                ImageUrl = "https://dotnetresturant.blob.core.windows.net/manger/pexels-muffin-creatives-1653877.jpg"
            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                Name = "Burger",
                Price = 12.50,
                CatagoryName = "Food",
                Description = "Juicy beef burger with cheese and veggies",
                ImageUrl = "https://dotnetresturant.blob.core.windows.net/manger/pexels-valeria-boltneva-1639565.jpg"
            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                Name = "Chocolate Cake",
                Price = 25.00,
                CatagoryName = "Dessert",
                Description = "Rich chocolate cake with a decadent frosting",
                ImageUrl = "https://dotnetresturant.blob.core.windows.net/manger/pexels-polina-tankilevitch-4109998.jpg"
            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 4,
                Name = "Cheesecake",
                Price = 20.00,
                CatagoryName = "Dessert",
                Description = "Creamy cheesecake with a graham cracker crust",
                ImageUrl = "https://dotnetresturant.blob.core.windows.net/manger/pexels-cesar-de-la-cruz-3185509.jpg"
            });

        }


    }
}
