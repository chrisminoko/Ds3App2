using Ds3App2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App2.Services.Profile
{
    public interface IProfile
    {
        Task<Customer> GetCustomer(string userId);
        Task UpdateCustomer(Customer customer);
        Task DeleteCustomer(string userId);
    }
}
