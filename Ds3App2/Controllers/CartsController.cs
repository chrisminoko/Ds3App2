using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ds3App2.Services.CartService;
using Microsoft.AspNet.Identity;
using Ds3App2.ViewModels;

namespace Ds3App2.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly ICart _cart;
        public CartsController(ICart cart)
        {
            _cart = cart;
        }
        public async Task<ActionResult> Index()
        {
            string userId = User.Identity.GetUserId();
            return View(await _cart.GetMyCart(userId));
        }

        [HttpGet]
        public async Task<ActionResult> Summary()
        {
            string userId = User.Identity.GetUserId();
            return View(await _cart.GetMyCart(userId));
        }

        public async Task<JsonResult> Create(Guid id)
        {
            string userId = User.Identity.GetUserId();
            await _cart.AddToCart(id, userId);
            return Json("Proccess completed successfuly", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> RemoveItem(Guid id)
        {
            string userId = User.Identity.GetUserId();
            await _cart.RemoveItemFromCart(id, userId);
            return Json("Proccess completed successfuly", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> BulkUpdate(Qty model)
        {
            string userId = User.Identity.GetUserId();
            await _cart.BulkQuantityUpdateItemForCart(Guid.Parse(model.id), userId, int.Parse(model.qty));
            return Json("Proccess completed successfuly", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> RemoveAll(Guid id)
        {
            string userId = User.Identity.GetUserId();
            await _cart.RemoveAllItems(id, userId);
            return Json("Proccess completed successfuly", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> ClearCart(string reference)
        {
            await _cart.ClearCart(reference);
            return Json("Proccess completed successfuly", JsonRequestBehavior.AllowGet);
        }
    }
}
