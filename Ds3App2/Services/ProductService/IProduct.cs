using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App2.Services.ProductService
{
    public interface IProduct
    {
        Task<IEnumerable<Models.Product>> GetProducts();
        Task CreateProduct(Models.Product product);
        Task<Models.Product> GetProductToEdit(Guid id);
        Task EditProduct(Models.Product product);
        Task DeleteProduct(Guid id);
    }
}
