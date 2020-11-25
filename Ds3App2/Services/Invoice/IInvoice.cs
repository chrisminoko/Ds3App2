using Ds3App2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App2.Services.Invoice
{
    public interface IInvoice
    {
        Task CreateInvoice(Guid id);
        Task<IEnumerable<Models.Invoice>> GetInvoices();
        Task<IEnumerable<Models.Invoice>> GetCustomerInvoices(string userId);
        Task<InvoiceViewModel> GetInvoice(Guid id);
        Task Paid(Guid id);
    }
}
