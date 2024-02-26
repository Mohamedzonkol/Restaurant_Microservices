using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Resturant.Services.Discount.DbContexts;
using Resturant.Services.Discount.Dtos;
using Resturant.Services.Discount.Models;

namespace Resturant.Services.Discount.Reposerty
{
    public class CopounRepoeserty :ICopounRepoeserty
    {
        private readonly AppDbContext _context;
        protected IMapper _mapper;
        public CopounRepoeserty(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CouponDto> GetCouponByCode(string CouponCode)
        {
            Coupon coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.CopounCode == CouponCode);
            return _mapper.Map<CouponDto>(coupon);
        }
    }
}
