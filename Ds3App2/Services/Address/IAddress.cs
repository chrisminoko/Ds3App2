using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App2.Services.Address
{
    public interface IAddress
    {
        Task<IEnumerable<Models.Address>> GetCustomerAddresses(string userId);
        Task AddAddress(Models.Address vehicle);
        Task<Models.Address> GetAddressToEdit(Guid id);
        Task EditAddress(Models.Address vehicle);
        Task DeleteAddress(Guid id);
    }
}
