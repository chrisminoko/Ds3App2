using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Ds3App2.Services.Profile
{
    public class Profile : IProfile
    {
        public async Task DeleteCustomer(string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var customer = await context.Customer.Where(c => c.UserId == userId).FirstOrDefaultAsync();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                await userManager.DeleteAsync(userManager.FindByIdAsync(userId).Result);
                context.Customer.Remove(customer);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Customer> GetCustomer(string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Customer.Where(c => c.UserId == userId).FirstOrDefaultAsync();
            }
        }

        public async Task UpdateCustomer(Customer customer)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Entry(customer).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}