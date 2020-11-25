using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ds3App2.DailyChecks;

namespace Ds3App2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    if (!Check.Checked())
                    {
                        Check.UpcomingBookings();
                    }
                }
            }
            return View();
        }

        public ActionResult BadRequest()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Success()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}