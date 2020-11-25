using Ds3App2.Services.Invoice;
using Ds3App2.Services.JobCard;
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
    public class JobCardController : Controller
    {
        private readonly IInvoice _invoice;
        private readonly IJobCard _job;
        public JobCardController(IInvoice invoice, IJobCard job)
        {
            _invoice = invoice;
            _job = job;
        }
        public async Task<ActionResult> CreateInvoice(Guid id)
        {
            try
            {
                await _invoice.CreateInvoice(id);
                return RedirectToAction("Index", "Invoice");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ActionResult> Create(Guid id)
        {
            try
            {
                await _job.CreateJobCard(id);
                var jobCard = await _job.GetJobCardTasks(id);
                return View(jobCard);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<JsonResult> Add(Guid id)
        {
            try
            {
                await _job.Add(id);
                return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<JsonResult> Minus(Guid id)
        {
            try
            {
                await _job.Minus(id);
                return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ActionResult> Next(Guid id)
        {
            try
            {
                var products = await _job.GetJobCardProducts(id);
                if (products.Count() > 0)
                {
                    return View(products);
                }
                else
                {
                    return RedirectToAction("Index", "Products");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ActionResult> GetJobCards(Guid? id)
        {
            try
            {
                if(id != null)
                {
                    await _job.CompleteCreatingAJobCard((Guid)id);
                }
                ViewBag.MechanicDropDown = new SelectList(await _job.GetMechanis(), "Id", "FirstName");
                return View(await _job.GetJobCards());
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ActionResult> GetForemeansJobCards()
        {
            try
            {
                string userId = User.Identity.GetUserId();
                return View(await _job.GetForemeansJobCards(userId));
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ActionResult> GetForemeansJobCardTasks(Guid id)
        {
            try
            {
                return View(await _job.GetForemeansJobCardTasks(id));
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> AddMechanic(string array)
        {
            string[] val = array.Split(',');
            await _job.AssignMechanic(Guid.Parse(val[0]), val[1]);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> Complete(Guid id)
        {
            await _job.Complete(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
    }
}