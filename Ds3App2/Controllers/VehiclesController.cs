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
using Ds3App2.Services.Vehicles;
using Microsoft.AspNet.Identity;

namespace Ds3App2.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly IVehicle _vehicle;
        public VehiclesController(IVehicle vehicle)
        {
            _vehicle = vehicle;
        }

        public async Task<ActionResult> Index()
        {
            string userId = User.Identity.GetUserId();
            return View(await _vehicle.GetCustomerVehicles(userId));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.CustomerId = User.Identity.GetUserId();
                vehicle.Id = Guid.NewGuid();
                await _vehicle.AddVehicle(vehicle);
                return RedirectToAction("Index");
            }

            return View(vehicle);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Vehicle vehicle = await _vehicle.GetVehicleToEdit((Guid)id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Models.Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                await _vehicle.EditVehicle(vehicle);
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        [HttpGet]
        public async Task<JsonResult> Delete(Guid id)
        {
            await _vehicle.DeleteVehicle(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
    }
}
