using Ds3App2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ds3App2.DomainLogic.Make
{
    public class Make
    {
        internal static async Task CreateMakeAsync(Models.Make make)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Make.Add(make);
                await context.SaveChangesAsync();
            }
        }
        internal static async Task DeleteMakeAsync(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Make.Remove(context.Make.Find(id));
                await context.SaveChangesAsync();
            }
        }

        internal async static Task EditMakeAsync(Models.Make make)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Entry(make).State = System.Data.Entity.EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        internal async static Task<Models.Make> GetMakeAsync(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Make.FindAsync(id);
            }
        }

        internal async static Task<List<Models.Make>> GetMakesAsync()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Make.ToListAsync();
            }
        }
    }
}