using Microsoft.EntityFrameworkCore;
using Resturant.Services.OrderApi.DbContexts;
using Resturant.Services.OrderApi.Models;

namespace Resturant.Services.OrderApi.Repoesetry
{
    public class OrderReposetry:IOrderReposetry
    {
        private readonly DbContextOptions<AppDbContext>_context;
        public OrderReposetry(DbContextOptions<AppDbContext> dbContext)
        {
            _context= dbContext;
        }
        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            await using var _db = new AppDbContext(_context);
            _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();
            return true;
        }

      

        public async Task UpdateOrderPayementStatus(int orderHeaderId, bool paid)
        {
            await using var _db = new AppDbContext(_context);
            var orderHeaderFromDb = await _db.OrderHeaders.FirstOrDefaultAsync(u => u.OrderHeaderId == orderHeaderId);
            if (orderHeaderFromDb != null)
            {
                orderHeaderFromDb.PayementStatus = paid;
                await _db.SaveChangesAsync();
            }
        }
    }
}
