using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Ds3App2.Models;
using Ds3App2.Services.ServiceType;

namespace Ds3App2.Controllers
{
    [Authorize]
    public class ServiceTypesController : Controller
    {
        private readonly IServiceType _service;
        public ServiceTypesController(IServiceType service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index()
        {
            try
            {
                return View(await _service.GetServiceTypes());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.ServiceType serviceType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    serviceType.Id = Guid.NewGuid();
                    await _service.CreateServiceType(serviceType);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return View(serviceType);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.ServiceType serviceType = await _service.GetServiceType((Guid)id);
            if (serviceType == null)
            {
                return HttpNotFound();
            }
            return View(serviceType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Models.ServiceType serviceType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.EditServiceType(serviceType);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(serviceType);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteServiceType(id);
                return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
