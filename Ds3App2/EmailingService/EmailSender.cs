using Ds3App2.Models;
using Ds3App2.Utilities;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Ds3App2.EmailingService
{
    public class EmailSender
    {
        private string Body = string.Empty;
        private async Task SendEmail(string HTMLTemplate, MailAddress emailAddress, string Subject)
        {
            var apiKey = ConfigurationManager.AppSettings["apikey"];
            var fromName = ConfigurationManager.AppSettings["sendGridUser"];
            var fromEmail = ConfigurationManager.AppSettings["sendGridFrom"];

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, fromName);
            var subject = Subject;
            var to = new EmailAddress(emailAddress.Address, "");
            var plainTextContent = "";
            var htmlContent = HTMLTemplate;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            await client.SendEmailAsync(msg);
        }

        public async Task ConfirmAccount(string email)
        {
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/Welcome.html")))
            {
                Body = reader.ReadToEnd();
            }
            Body = Body.Replace("{Header}", "New Account");
            Body = Body.Replace("{Message}", "A new account has been created please use your email and password below to login.");
            Body = Body.Replace("{Password}", "Password01");

            await SendEmail(Body, new MailAddress(email), "New Account");
        }
        public async Task LowStock(Models.Product product)
        {
            using (StreamReader reader = new StreamReader(Path.Combine(HttpRuntime.AppDomainAppPath, "EmailTemplate/LowStock.html")))
            {
                Body = reader.ReadToEnd();
            }
            Body = Body.Replace("{Header}", "Low Stock");
            Body = Body.Replace("{Message}", $"Product: {product.ProductName}" + "<br />" +
                $"Brand: {product.Brand}"  + "<br />" +
                $"Quantity: {product.StockQuantity}" +
                $" remainig stock is below the acceptable stock level, please place an order with the supplier.");

            await SendEmail(Body, new MailAddress(WebConfigurationSettings.LowStockEmailTo), $"Low Stock {product.ProductName}");
        }
        public async Task BookingReminder(Models.Booking booking)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                var customer = context.Customer.Where(c => c.UserId == booking.CustomerId)?.FirstOrDefault();
                var vehicleId = context.BookedVehicles.Where(v => v.Reference == booking.Reference)
                    .FirstOrDefault().VehilcleId;
                var vehicle = context.Vehicles.Find(vehicleId);
                using (StreamReader reader = new StreamReader(Path.Combine(HttpRuntime.AppDomainAppPath, "EmailTemplate/UpcomingBooking.html")))
                {
                    Body = reader.ReadToEnd();
                }
                Body = Body.Replace("{Header}", $"Booking Reminder : #{booking.Reference}");
                Body = Body.Replace("{Customer}", $"Dear {customer.FirstName} {customer.LastName}");
                Body = Body.Replace("{Message}", $"This is a friendly reminder that vehicle {vehicle.RegistrationNumber}" +
                    $" is booked for {booking.ServiceType} Service at {booking.Mileage} Km's the service is tomorrow.");

                await SendEmail(Body, new MailAddress(customer.Email), "Booking Reminder");

            }
        }
    }
   
}