using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App2.Models;

namespace Ds3App2.Services.ProductService
{
    public class Product : IProduct
    {
        public async Task CreateProduct(Models.Product product)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                try
                {
                    product.Id = Guid.NewGuid();
                    context.Product.Add(product);
                    await context.SaveChangesAsync();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public async Task DeleteProduct(Guid id)
        {
             using (ApplicationDbContext context = new ApplicationDbContext())
             {
                try
                {
                    context.Product.Find(id).IsDeleted = true;
                    await context.SaveChangesAsync();
                }
                catch (Exception)
                {

                    throw;
                }
             }
        }

        public async Task EditProduct(Models.Product product)
        {
             using (ApplicationDbContext context = new ApplicationDbContext())
             {
                try
                {
                    context.Entry(product).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                catch (Exception)
                {

                    throw;
                }
             }
        }

        public async Task<IEnumerable<Models.Product>> GetProducts()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                try
                {
                    return await context.Product.Where(p => !p.IsDeleted).OrderBy(p => p.ProductName).ToListAsync();

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public async Task<Models.Product> GetProductToEdit(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                try
                {
                    return await context.Product.FindAsync(id);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}