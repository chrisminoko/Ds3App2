using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App2.Extensions;
using Ds3App2.Models;

namespace Ds3App2.Services.CartService
{
    public class Cart : ICart
    {
        public async Task AddToCart(Guid Id, string userId)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                Product product = context.Product.Find(Id);
                if(product != null)
                {
                    string reference = "";
                    if(context.Carts.Any(c => c.CustomerId == userId && !c.IsComplete))
                    {
                        reference = context.Carts.Where(c => c.CustomerId == userId && !c.IsComplete).FirstOrDefault().Reference;
                    }
                    else
                    {
                        reference = Helper.GetReference();
                    }
                    if(context.Carts.Any(c => c.CustomerId == userId && c.ProductId == Id && !c.IsComplete))
                    {
                        var up = context.Carts.Where(c => c.CustomerId == userId && c.ProductId == Id && !c.IsComplete).FirstOrDefault();
                        up.Quantity += 1;
                    }
                    else
                    {
                        Models.Cart cart = new Models.Cart()
                        {
                            Id = Guid.NewGuid(),
                            Image = product.ProductImage,
                            Product = product.ProductName,
                            Price = product.Price,
                            Quantity = 1,
                            CustomerId = userId,
                            Reference = reference,
                            ProductId = Id
                        };
                        context.Carts.Add(cart);
                    }

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task BulkQuantityUpdateItemForCart(Guid Id, string userId, int qty)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Product product = context.Product.Find(Id);
                if (product != null)
                {
                    var up = context.Carts.Where(c => c.CustomerId == userId && c.ProductId == Id && !c.IsComplete).FirstOrDefault();
                    up.Quantity = qty;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task ClearCart(string reference)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                foreach (var item in await context.Carts.Where(c => c.Reference == reference).ToListAsync())
                {
                    context.Carts.Remove(context.Carts.Find(item.Id));
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Models.Cart>> GetMyCart(string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Carts.Where(c => c.CustomerId == userId &&
                !c.IsComplete).ToListAsync();
            }
        }

        public async Task RemoveAllItems(Guid Id, string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Product product = context.Product.Find(Id);
                if (product != null)
                {
                    var up = context.Carts.Where(c => c.CustomerId == userId && c.ProductId == Id && !c.IsComplete).FirstOrDefault();
                    context.Carts.Remove(up);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveItemFromCart(Guid Id, string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Product product = context.Product.Find(Id);
                if (product != null)
                {
                    var up = context.Carts.Where(c => c.CustomerId == userId && c.ProductId == Id && !c.IsComplete).FirstOrDefault();
                    if(up.Quantity == 1)
                    {
                        context.Carts.Remove(up);
                    }
                    else
                    {
                        up.Quantity -= 1;
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}