using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App2.Services.CartService
{
    public interface ICart
    {
        Task ClearCart(string reference);
        Task RemoveAllItems(Guid Id, string userId);
        Task AddToCart(Guid Id, string userId);
        Task RemoveItemFromCart(Guid Id, string userId);
        Task BulkQuantityUpdateItemForCart(Guid Id, string userId, int qty);
        Task<IEnumerable<Models.Cart>> GetMyCart(string userId);
    }
}
