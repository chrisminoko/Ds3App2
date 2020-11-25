using Ds3App2.Models;
using Ds3App2.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ds3App2.Services.Invoice
{
    public class Invoice : IInvoice
    {
        public async Task CreateInvoice(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var jobCard = context.JobCard.Find(id);
                    var bookingId = Guid.Parse(jobCard.BookingIdString);
                    Models.Invoice invoice = new Models.Invoice();
                    decimal totalCostOfLabour = 0, totalCostOfMaterial = 0;
                    invoice.Id = Guid.NewGuid();
                    invoice.Reference = jobCard.Reference;
                    invoice.TotalTasks = context.JobCardTask.Count(m => !m.IsDeleted && m.BookingId == bookingId);
                    invoice.TotalMaterial = context.JobCardProduct.Count(m => !m.IsDeleted && m.BookingId == bookingId);
                    foreach (var item in context.JobCardTask.Where(m => !m.IsDeleted && m.BookingId == bookingId).ToList())
                    {
                        totalCostOfLabour += context.JobTask.Find(item.JobTask).RatePerHour;
                    }
                    foreach (var item in context.JobCardProduct.Where(m => !m.IsDeleted && m.BookingId == bookingId).ToList())
                    {
                        totalCostOfMaterial += context.Product.Find(item.JobProduct).Price;
                    }
                    invoice.CostOfLabour = totalCostOfLabour;
                    invoice.CostOfMaterial = totalCostOfMaterial;
                    invoice.InvoiceAmount = totalCostOfLabour + totalCostOfMaterial;
                    invoice.CustomerId = context.Bookings
                        .Where(b => b.Id == bookingId)
                        .FirstOrDefault().CustomerId;
                    invoice.Paid = false;
                    invoice.DateTimeStamp = DateTime.Now;

                    context.Invoice.Add(invoice);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Models.Invoice>> GetCustomerInvoices(string userId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return await context.Invoice.Where(i => i.CustomerId == userId)
                        .ToListAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<InvoiceViewModel> GetInvoice(Guid id)
        {
            try
            {
                return await BLL.Invoice.GetInvoiceDetails(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Models.Invoice>> GetInvoices()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return await context.Invoice
                        .ToListAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Paid(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    context.Invoice.Find(id).Paid = true;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}