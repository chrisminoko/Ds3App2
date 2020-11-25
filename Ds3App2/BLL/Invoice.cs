using Ds3App2.Models;
using Ds3App2.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ds3App2.BLL
{
    public class Invoice
    {
        internal static async Task<InvoiceViewModel> GetInvoiceDetails(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await GetDetails(id, context);
            }
        }

        private static async Task<InvoiceViewModel> GetDetails(Guid id, ApplicationDbContext context)
        {
            var invoiceDetails = new InvoiceViewModel();
            var invoice = await context.Invoice.FindAsync(id);
            var customer = await context.Customer.Where(c => c.UserId == invoice.CustomerId).FirstOrDefaultAsync();
            var vehicleBooked = await context.BookedVehicles.Where(v => v.Reference == invoice.Reference).FirstOrDefaultAsync();
            var vehicle = await context.Vehicles.FindAsync(vehicleBooked.VehilcleId);
            var address = await context.Addresses.Where(a => a.CustomerId == customer.UserId && !a.IsDeleted).FirstOrDefaultAsync();

            invoiceDetails.Invoice = invoice;
            invoiceDetails.Customer = customer;
            invoiceDetails.Vehicle = vehicle;
            invoiceDetails.Address = address;
            return invoiceDetails;
        }
    }
}