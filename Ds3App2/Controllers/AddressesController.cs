using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Ds3App2.Services.Address;
using Microsoft.AspNet.Identity;

namespace Ds3App2.Controllers
{
    [Authorize]
    public class AddressesController : Controller
    {
        private readonly IAddress _address;
        public AddressesController(IAddress address)
        {
            _address = address;
        }

        public async Task<ActionResult> Index()
        {
            string userId = User.Identity.GetUserId();
            return View(await _address.GetCustomerAddresses(userId));
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.Address address)
        {
            if (ModelState.IsValid)
            {
                address.CustomerId = User.Identity.GetUserId();
                address.Id = Guid.NewGuid();
                await _address.AddAddress(address);
                return RedirectToAction("Index");
            }

            return View(address);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Address address = await _address.GetAddressToEdit((Guid)id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Models.Address address)
        {
            if (ModelState.IsValid)
            {
                await _address.EditAddress(address);
                return RedirectToAction("Index");
            }
            return View(address);
        }

        [HttpGet]
        public async Task<JsonResult> Delete(Guid id)
        {
            await _address.DeleteAddress(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
    }
}
