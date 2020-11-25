using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App2.Models;

namespace Ds3App2.Services.Address
{
    public class Address : IAddress
    {
        public async Task AddAddress(Models.Address address)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Addresses.Add(address);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAddress(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Addresses.Remove(context.Addresses.Find(id));
                await context.SaveChangesAsync();
            }
        }

        public async Task EditAddress(Models.Address address)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Entry(address).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task<Models.Address> GetAddressToEdit(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Addresses.FindAsync(id);
            }
        }

        public async Task<IEnumerable<Models.Address>> GetCustomerAddresses(string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Addresses.Where(a => !a.IsDeleted &&
                a.CustomerId == userId)
                    .ToListAsync();
            }
        }
    }
}