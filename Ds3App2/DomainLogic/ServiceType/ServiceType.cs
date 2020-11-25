using Ds3App2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ds3App2.DomainLogic.ServiceType
{
    public class ServiceType
    {
        internal static async Task CreateServiceTypeAsync(Models.ServiceType service)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.ServiceType.Add(service);
                await context.SaveChangesAsync();
            }
        }
        internal static async Task DeleteServiceTypeAsync(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.ServiceType.Remove(context.ServiceType.Find(id));
                await context.SaveChangesAsync();
            }
        }

        internal async static Task EditServiceTypeAsync(Models.ServiceType service)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Entry(service).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        internal async static Task<Models.ServiceType> GetServiceTypeAsync(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.ServiceType.FindAsync(id);
            }
        }

        internal async static Task<List<Models.ServiceType>> GetServiceTypesAsync()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.ServiceType.ToListAsync();
            }
        }
    }
}