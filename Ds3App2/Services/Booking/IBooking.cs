using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ds3App2.Services.Booking
{
    public interface IBooking
    {
        Task<IEnumerable<Models.Booking>> GetBookings(string search);
        Task<IEnumerable<Models.Booking>> GetBookings(string userId, string search);
        Task CreateBooking(Models.Booking booking);
        Task<IEnumerable<Models.ServiceType>> GetServiceTypes();
        Task CreateVehicle(Models.Vehicle vehicle, string reference);
        Task Approve(Guid id);
        Task Cancel(Guid id);
        Task CheckIn(Guid id);
        Task CheckOut(Guid id);
        Task Delete(Guid id);
    }
}
