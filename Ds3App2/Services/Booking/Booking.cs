using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App2.Models;

namespace Ds3App2.Services.Booking
{
    public class Booking : IBooking
    {
        public async Task Approve(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var update = context.Bookings.Find(id);
                    update.Status = "Approved";
                    context.Entry(update).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Cancel(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    context.Bookings.Find(id).Status = "Cancelled";
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task CheckIn(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Bookings.Find(id).CheckIn = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task CheckOut(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Bookings.Find(id).CheckOut = true;
                await context.SaveChangesAsync();
                DeleteMechanicFromBooking(context, id);
            }
        }

        private void DeleteMechanicFromBooking(ApplicationDbContext context, Guid id)
        {
            var mechanicBookings = context.MechanicsBooking
                .Where(x => x.BookingId == id).ToList();

            foreach (var mechanic in mechanicBookings)
            {
                context.MechanicsBooking.Remove(mechanic);
                context.SaveChanges();
            }

        }

        public async Task CreateBooking(Models.Booking booking)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var findVehicleId = context.Vehicles.Where(v => v.RegistrationNumber
                    == booking.Reference).FirstOrDefault();
                    var data = context.BookedVehicles.Where(b => b.VehilcleId == findVehicleId.Id).FirstOrDefault();
                    booking.Reference = data.Reference;
                    findVehicleId.CustomerId = booking.CustomerId;
                    context.Bookings.Add(booking);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task CreateVehicle(Vehicle vehicle, string reference)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    context.BookedVehicles.Add(new BookedVehicle()
                    {
                        Id = Guid.NewGuid(),
                        VehilcleId = vehicle.Id,
                        Reference = reference
                    });
                    context.Vehicles.Add(vehicle);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var booking = context.Bookings.Find(id);
                if(booking != null)
                {
                    context.Bookings.Remove(booking);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<Models.Booking>> GetBookings(string userId, string search)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    if (!string.IsNullOrEmpty(search))
                    {
                        return await context.Bookings.Where(a => !a.IsDeleted &&
                        a.CustomerId == userId
                        && a.Reference.Contains(search.ToUpper()))
                       .OrderByDescending(a => a.DateTimeStamp)
                       .ToListAsync();
                    }
                    else
                    {
                        return await context.Bookings.Where(a => !a.IsDeleted &&
                        a.CustomerId == userId)
                       .OrderByDescending(a => a.DateTimeStamp)
                       .ToListAsync();
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Models.Booking>> GetBookings(string search)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    if (!string.IsNullOrEmpty(search))
                    {
                        return await context.Bookings.Where(a => !a.IsDeleted
                        && a.Date >= DateTime.Today
                        && a.Reference.Contains(search.ToUpper()))
                       .OrderByDescending(a => a.DateTimeStamp)
                       .ToListAsync();
                    }
                    else
                    {
                        return await context.Bookings.Where(a => !a.IsDeleted && a.Date >= DateTime.Today)
                       .OrderByDescending(a => a.DateTimeStamp)
                       .ToListAsync();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Models.ServiceType>> GetServiceTypes()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return await context.ServiceType
                        .ToListAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}