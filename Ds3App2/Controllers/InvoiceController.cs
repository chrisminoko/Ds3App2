using Ds3App2.Services.Invoice;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ds3App2.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly IInvoice _invoice;
        public InvoiceController(IInvoice invoice)
        {
            _invoice = invoice;
        }

        public async Task<ActionResult> Index()
        {
            return View(await _invoice.GetInvoices());
        }

        public async Task<ActionResult> IndexCustomer()
        {
            var userId = User.Identity.GetUserId();
            return View(await _invoice.GetCustomerInvoices(userId));
        }

        public async Task<ActionResult> Details(Guid id)
        {
            return View(await _invoice.GetInvoice(id));
        }

        public async Task<ActionResult> CustomerDetails(Guid id)
        {
            return View(await _invoice.GetInvoice(id));
        }

        [HttpGet]
        public async Task<JsonResult> Paid(Guid id)
        {
            await _invoice.Paid(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
    }
}