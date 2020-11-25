using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ds3App2.Models;
using Ds3App2.Services.Booking;
using Microsoft.AspNet.Identity;
using Ds3App2.Extensions;
using Ds3App2.Services.Make;
using Ds3App2.Services.Invoice;

namespace Ds3App2.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly IBooking _booking;
        private readonly IMake _make;

        public BookingsController(IBooking booking, IMake make)
        {
            _booking = booking;
            _make = make;
        }

        public async Task<ActionResult> IndexAdmin(string search = null)
        {
            var bookings = await _booking.GetBookings(search);
            return View(bookings);
        }

        public async Task<ActionResult> Index(string search = null)
        {
            string userId = User.Identity.GetUserId();
            return View(await _booking.GetBookings(userId,search));
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.CarMakes = new SelectList(await _make.GetMakes(), "Name", "Name");
            ViewBag.Services = new SelectList(await _booking.GetServiceTypes(), "Type", "Type");
            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.Booking booking)
        {
            if (ModelState.IsValid)
            {
				

				//Sms sms = new Sms();
				//HttpCookie myCookie = new HttpCookie("MyCookie");
				//myCookie = Request.Cookies["MyCookie"];
				//int Id = Convert.ToInt16(myCookie);

				//try
				//{
				//	sms.Send_SMS(booking.PhoneNumber, "Hi, your booking has been successfully captured. Please consistantly check your status");
				//}
				//catch
				//{
				//	ViewBag.Error = "Invalid network";
				//}

				booking.DateTimeStamp = DateTime.Now;
                booking.Id = Guid.NewGuid();
                booking.CustomerId = User.Identity.GetUserId();
                booking.Status = "Pending";
                await _booking.CreateBooking(booking);
                return RedirectToAction("Index");
            }
            ViewBag.CarMakes = new SelectList(await _make.GetMakes(), "Name", "Name");
            ViewBag.Services = new SelectList(await _booking.GetServiceTypes(), "Type", "Type");
            return View(booking);
        }

        [HttpPost]
        public async Task<JsonResult> AddVehicle(V data)
        {
            string reference = Helper.GetReference();
            TempData["ID"] = reference;
            await _booking.CreateVehicle(CreateVehicle(data), reference);
            return Json(new object[] { data.regNumber }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Approve(Guid id)
        {
			//using (ApplicationDbContext db = new ApplicationDbContext())
			//{
			//	Models.Booking booking = new Models.Booking();

			//	string num = (from ac in db.Bookings
			//				  where ac.Id == id
			//				  select ac.PhoneNumber).FirstOrDefault();

			//	DateTime date = (from ac in db.Bookings
			//					 where ac.Id == id
			//					 select ac.Date).FirstOrDefault();

			//	Sms sms = new Sms();
			//	HttpCookie myCookie = new HttpCookie("MyCookie");
			//	myCookie = Request.Cookies["MyCookie"];
			//	int Id = Convert.ToInt16(myCookie);

			//	try
			//	{
			//		sms.Send_SMS(num, "Hi , your booking for the date, " + date + " has been approved. Thank You");
			//	}
			//	catch
			//	{
			//		ViewBag.Error = "Invalid network";
			//	}

				await _booking.Approve(id);
                return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
            //}
        }

        private Vehicle CreateVehicle(V data)
        {
            Vehicle vehicle = new Vehicle()
            {
                Id = Guid.NewGuid(),
                Make = data.make,
                Model = data.model,
                Colour = data.colour,
                RegistrationNumber = data.regNumber,
            };

            return vehicle;
        }

        [HttpPost]
        public async Task<JsonResult> CheckIn(Guid id)
        {
            await _booking.CheckIn(id);
            return Json("Checked in.", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<JsonResult> CheckOut(Guid id)
        {
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            //    Models.Booking booking = new Models.Booking();

            //    string num = (from ac in db.Bookings
            //                  where ac.Id == id
            //                  select ac.PhoneNumber).FirstOrDefault();

            //    DateTime date = (from ac in db.Bookings
            //                     where ac.Id == id
            //                     select ac.Date).FirstOrDefault();

            //    Sms sms = new Sms();
            //    HttpCookie myCookie = new HttpCookie("MyCookie");
            //    myCookie = Request.Cookies["MyCookie"];
            //    int Id = Convert.ToInt16(myCookie);

            //    try
            //    {
            //        sms.Send_SMS(num, "Your car has been succesfully serviced.");
            //    }
            //    catch
            //    {
            //        ViewBag.Error = "Invalid network";
            //    }
                await _booking.CheckOut(id);
                return Json("Checked out.", JsonRequestBehavior.AllowGet);
            //}
        }

        [HttpPost]
        public async Task<JsonResult> Delete(Guid id)
        {
            await _booking.Delete(id);
            return Json("Checked out.", JsonRequestBehavior.AllowGet);
        }
    }

    public class V
    {
        public string make { get; set; }
        public string model { get; set; }
        public string colour { get; set; }
        public string regNumber { get; set; }
    }
}
