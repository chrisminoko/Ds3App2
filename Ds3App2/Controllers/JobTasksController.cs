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
using Ds3App2.Services.JobCard;

namespace Ds3App2.Controllers
{
    [Authorize]
    public class JobTasksController : Controller
    {
        private readonly IJobCard _job;
        public JobTasksController(IJobCard job)
        {
            _job = job;
        }
        public async Task<ActionResult> Index()
        {
            return View(await _job.GetJobCardTasks());
        }
        public async Task<ActionResult> Create()
        {
            ViewBag.ServiceType = new SelectList(await _job.GetServiceTypes(), "Id", "Type");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobTask jobTask)
        {
            if (ModelState.IsValid)
            {
                jobTask.Id = Guid.NewGuid();
                await _job.AddJobCardTask(jobTask);
                return RedirectToAction("Index");
            }
            if(jobTask.ServiceType == Guid.Empty)
            {
                ViewBag.ServiceTypeDropDown = new SelectList(await _job.GetServiceTypes(), "Id", "Type");
            }
            else
            {
                ViewBag.ServiceTypeDropDown = new SelectList(await _job.GetServiceTypes(), "Id", "Type", jobTask.ServiceType);
            }
            return View(jobTask);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTask jobTask = await _job.GetJobCardTaskToEdit((Guid)id);
            if (jobTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceTypeDropDown = new SelectList(await _job.GetServiceTypes(), "Id", "Type", jobTask.ServiceType);

            return View(jobTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobTask jobTask)
        {
            ViewBag.ServiceTypeDropDown = new SelectList(await _job.GetServiceTypes(), "Id", "Type", jobTask.ServiceType);

            if (ModelState.IsValid)
            {
                await _job.EditJobCardTask(jobTask);
                return RedirectToAction("Index");
            }
            return View(jobTask);
        }
        [HttpGet]
        public async Task<JsonResult> Delete(Guid id)
        {
            await _job.DeleteJobCardTask(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
    }
}
