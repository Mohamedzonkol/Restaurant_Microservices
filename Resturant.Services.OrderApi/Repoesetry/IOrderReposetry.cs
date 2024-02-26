using Resturant.Services.OrderApi.Models;

namespace Resturant.Services.OrderApi.Repoesetry
{
    public interface IOrderReposetry
    {
        Task<bool> AddOrder(OrderHeader orderHeader);
        Task UpdateOrderPayementStatus(int orderHeaderId,bool Paid);
    }
}
