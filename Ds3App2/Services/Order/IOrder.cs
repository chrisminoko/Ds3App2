using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ds3App2.Services.Order
{
    public interface IOrder
    {
        Task CreateOrder(string reference);
        Task<IEnumerable<Models.Order>> GetOrders();
        Task<IEnumerable<Models.Order>> GetCustomerOrders(string userId);
        Task<Models.Order> GetOrder(Guid id);
        Task Update(Guid id);
    }
}
