using Microsoft.EntityFrameworkCore;
using Resturant.Services.Discount.Models;

namespace Resturant.Services.Discount.DbContexts
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Coupon>Coupons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 1,
                CopounCode = "10FS0",
                DiscountAmount = 10
            });  modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 2,
                CopounCode = "10FF0",
                DiscountAmount = 20
            });  modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 3,
                CopounCode = "10FFF",
                DiscountAmount = 50
            });
        }
    }
}
