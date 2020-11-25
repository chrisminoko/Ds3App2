using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ds3App2.Models;
using Ds3App2.Services.Profile;
using Microsoft.AspNet.Identity;
using Ds3App2.Structs;

namespace Ds3App2.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly IProfile _profile;
        public CustomersController(IProfile profile)
        {
            _profile = profile;
        }

        public async Task<ActionResult> Edit()
        {
            string userId = User.Identity.GetUserId();
            return View(await _profile.GetCustomer(userId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _profile.UpdateCustomer(customer);
                ViewBag.Success = ResponseMessages.CustomerDetailsUpdate;
            }
            return View(customer);
        }

        public async Task<JsonResult> Delete()
        {
            string userId = User.Identity.GetUserId();
            await _profile.DeleteCustomer(userId);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
    }
}
