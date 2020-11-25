using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App2.Models;

namespace Ds3App2.Services.Vehicles
{
    public class Vehicle : IVehicle
    {
        public async Task AddVehicle(Models.Vehicle vehicle)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Vehicles.Add(vehicle);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteVehicle(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Vehicles.Remove(context.Vehicles.Find(id));
                await context.SaveChangesAsync();
            }
        }

        public async Task EditVehicle(Models.Vehicle vehicle)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Entry(vehicle).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Models.Vehicle>> GetCustomerVehicles(string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Vehicles.Where(a => !a.IsDeleted &&
                a.CustomerId == userId)
                    .ToListAsync();
            }
        }

        public async Task<Models.Vehicle> GetVehicleToEdit(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
               return await context.Vehicles.FindAsync(id);
            }
        }
    }
}