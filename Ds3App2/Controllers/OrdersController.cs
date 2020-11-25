using Ds3App2.Services.Order;
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
    public class OrdersController : Controller
    {
        private readonly IOrder _order;
        public OrdersController(IOrder order)
        {
            _order = order;
        }

        public async Task<ActionResult> Index(string reference = null)
        {
            string userId = User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(reference))
            {
                await _order.CreateOrder(reference);
            }
            return View(await _order.GetCustomerOrders(userId));
        }
        public async Task<ActionResult> IndexAdmin()
        {
            return View(await _order.GetOrders());
        }

        public async Task<ActionResult> Track(Guid id)
        {
            return View(await _order.GetOrder(id));
        }
        public async Task<ActionResult> GetOrder(Guid id)
        {
            return View(await _order.GetOrder(id));
        }

        public async Task<JsonResult> Update(Guid id)
        {
            await _order.Update(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
    }
}