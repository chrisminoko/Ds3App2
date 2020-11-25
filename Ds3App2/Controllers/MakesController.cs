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
using Ds3App2.Services.Make;

namespace Ds3App2.Controllers
{
    [Authorize]
    public class MakesController : Controller
    {
        private readonly IMake _make;
        public MakesController(IMake make)
        {
            _make = make;
        }
        public async Task<ActionResult> Index()
        {
            try
            {
                return View(await _make.GetMakes());
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
        public async Task<ActionResult> Create(Models.Make make)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    make.Id = Guid.NewGuid();
                    await _make.CreateMake(make);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return View(make);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Make make = await _make.GetMake((Guid)id);
            if (make == null)
            {
                return HttpNotFound();
            }
            return View(make);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Models.Make make)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _make.EditMake(make);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(make);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _make.DeleteMake(id);
                return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
