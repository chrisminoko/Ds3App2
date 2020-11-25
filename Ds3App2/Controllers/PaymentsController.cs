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

namespace Ds3App2.Controllers
{
    public class PaymentsController : Controller
    {
        public async Task<ActionResult> GetPayments()
        {
            return View();
        }
    }
}
