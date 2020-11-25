using Ds3App2.EmailingService;
using Ds3App2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ds3App2.DailyChecks
{
    public class Check
    {
        private static EmailSender sender = new EmailSender();
        private static DateTime UpcomingBookingsDate = DateTime.Today.AddDays(1);
        public static bool Checked()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.DailyCheck.Where(b => b.DateChecked == DateTime.Today
                && b.Checked)
                .Any();
            }
        }
        public static void UpcomingBookings()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var upcomingBooking = context.Bookings.Where(b => !b.IsComplete
                && !b.IsDeleted && b.Date == UpcomingBookingsDate)
                .ToList();
                foreach (var item in upcomingBooking)
                {
                    SendNotificationToCustomer(item, context);
                }
            }
        }

        private static void SendNotificationToCustomer(Booking booking, ApplicationDbContext context)
        {
            Task.Run(async ()=> await sender.BookingReminder(booking));
            UpdateDailyChecks(context);
        }
        private static void UpdateDailyChecks(ApplicationDbContext context)
        {
            DailyCheck check = new DailyCheck()
            {
                Checked = true
            };
            context.DailyCheck.Add(check);
            context.SaveChanges();
        }
    }
}