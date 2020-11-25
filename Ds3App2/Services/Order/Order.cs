using Ds3App2.EmailingService;
using Ds3App2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ds3App2.Services.Order
{
    public class Order : IOrder
    {

        public async Task CreateOrder(string reference)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    Payment payment = new Payment();
                    Models.Order order = new Models.Order();
                    order.Id = Guid.NewGuid();
                    order.Reference = reference;
                    var cart = context.Carts.Where(c => c.Reference == reference).ToList();
                    order.CustomerId = cart.FirstOrDefault().CustomerId;
                    var customer = context.Customer.Where(c => c.UserId == order.CustomerId).FirstOrDefault();

                    foreach (var item in cart)
                    {
                        EmailSender sender = new EmailSender();
                        order.Amount += item.Quantity * item.Price;
                        context.Carts.Find(item.Id).IsComplete = true;
                        var product = context.Product.Find(item.ProductId);
                        product.StockQuantity -= item.Quantity;
                        if(product.StockQuantity <= product.LowLevel)
                        {
                            await sender.LowStock(product);
                        }
                    }
                    order.OrderDate = DateTime.Now;
                    order.Status = "Paid";
                    order.IsDeleted = false;
                    order.IsDelivery = false;
                
                    context.Order.Add(order);

                    //add payment 
                    payment.CustomerName = customer.FirstName;
                    payment.CustomerSurname = customer.LastName;
                    payment.CustomerEmail = customer.Email;
                    payment.OrderType = "Online - Parts Purchase";
                    payment.OrderReference = order.Reference;
                    payment.Amount = order.Amount.ToString("C");
                    context.Payment.Add(payment);
                    //end
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<Models.Order>> GetCustomerOrders(string userId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return await context.Order.Where(o => !o.IsDeleted
                    && o.CustomerId == userId).ToListAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Models.Order> GetOrder(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return await context.Order.FindAsync(id);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Models.Order>> GetOrders()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return await context.Order.Where(o => !o.IsDeleted).ToListAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Update(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var order = context.Order.Find(id);
                    if (string.IsNullOrEmpty(order.StatusTraking))
                    {
                        order.StatusTraking = "WithCourier";
                    }
                    else if(order.StatusTraking == "WithCourier")
                    {
                        order.StatusTraking = "OutOnDelivery";
                    }
                    else if (order.StatusTraking == "OutOnDelivery")
                    {
                        order.StatusTraking = "Delivered";
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}