using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App2.Services.Vehicles
{
    public interface IVehicle
    {
        Task<IEnumerable<Models.Vehicle>> GetCustomerVehicles(string userId);
        Task AddVehicle(Models.Vehicle vehicle);
        Task<Models.Vehicle> GetVehicleToEdit(Guid id);
        Task EditVehicle(Models.Vehicle vehicle);
        Task DeleteVehicle(Guid id);
    }
}
