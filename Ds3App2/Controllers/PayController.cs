using Ds3App2.Models;
using Ds3App2.Services.Order;
using Ds3App2.Services.Pay;
using Ds3App2.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ds3App2.Controllers
{
    public class PayController : Controller
    {
        private ApplicationDbContext _db;
        private IPayment _payment;
        private readonly IOrder _order;
        private readonly string _payGateID;
        private readonly string _payGateKey;

        public PayController(IPayment payment, IOrder order)
        {
            _payment = payment;
            _order = order;
            _payGateID = WebConfigurationSettings.PayGateId;
            _payGateKey = WebConfigurationSettings.PayGateKey;
            _db = new ApplicationDbContext();
        }
    

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetRequest(string reference)
        {
            decimal amount = 0;
            string email = User.Identity.Name;
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                foreach (var item in context.Carts.Where(c => c.Reference == reference &&
                !c.IsDeleted && !c.IsComplete).ToList())
                {
                    amount += item.Price * (decimal)item.Quantity;
                }
            }
            HttpClient http = new HttpClient();
            Dictionary<string, string> request = new Dictionary<string, string>();
            string paymentAmount = (amount * 100).ToString("00"); // amount int cents e.i 50 rands is 5000 cents

            request.Add("PAYGATE_ID", _payGateID);
            request.Add("REFERENCE", reference); // Payment ref e.g ORDER NUMBER
            request.Add("AMOUNT", paymentAmount);
            request.Add("CURRENCY", "ZAR"); // South Africa
            request.Add("RETURN_URL", $"{Request.Url.Scheme}://{Request.Url.Authority}/pay/completepayment");
            request.Add("TRANSACTION_DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            request.Add("LOCALE", "en-za");
            request.Add("COUNTRY", "ZAF");

            request.Add("EMAIL", email);

            request.Add("CHECKSUM", _payment.GetMd5Hash(request, _payGateKey));

            string requestString = _payment.ToUrlEncodedString(request);
            StringContent content = new StringContent(requestString, Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage response = await http.PostAsync("https://secure.paygate.co.za/payweb3/initiate.trans", content);
            // if the request did not succeed, this line will make the program crash
            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();

            Dictionary<string, string> results = _payment.ToDictionary(responseContent);

            if (results.Keys.Contains("ERROR"))
            {
                return Json(new
                {
                    success = false,
                    message = "An error occured while initiating your request"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!_payment.VerifyMd5Hash(results, _payGateKey, results["CHECKSUM"]))
            {
                return Json(new
                {
                    success = false,
                    message = "MD5 verification failed"
                }, JsonRequestBehavior.AllowGet);
            }

            bool IsRecorded = _payment.AddTransaction(request, results["PAY_REQUEST_ID"]);
            if (IsRecorded)
            {
                return Json(new
                {
                    success = true,
                    message = "Request completed successfully",
                    results
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                success = false,
                message = "Failed to record a transaction"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CompletePayment()
        {
            string responseContent = Request.Params.ToString();
            Dictionary<string, string> results = _payment.ToDictionary(responseContent);

            Transaction transaction = _payment.GetTransaction(results["PAY_REQUEST_ID"]);

            if (transaction == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }
            Dictionary<string, string> validationSet = new Dictionary<string, string>();
            validationSet.Add("PAYGATE_ID", _payGateID);
            validationSet.Add("PAY_REQUEST_ID", results["PAY_REQUEST_ID"]);
            validationSet.Add("TRANSACTION_STATUS", results["TRANSACTION_STATUS"]);
            validationSet.Add("REFERENCE", transaction.REFERENCE);

            if (!_payment.VerifyMd5Hash(validationSet, _payGateKey, results["CHECKSUM"]))
            {
                return RedirectToAction("BadRequest", "Home");
            }

            int paymentStatus = int.Parse(results["TRANSACTION_STATUS"]);
            if (paymentStatus == 1)
            {
                await VerifyTransaction(responseContent, transaction.REFERENCE);
                return RedirectToAction("Complete", new { id = results["TRANSACTION_STATUS"], reference = transaction.REFERENCE });
            }
            return RedirectToAction("BadRequest", "Home");
        }

        private async Task VerifyTransaction(string responseContent, string Referrence)
        {
            HttpClient client = new HttpClient();
            Dictionary<string, string> response = _payment.ToDictionary(responseContent);
            Dictionary<string, string> request = new Dictionary<string, string>();

            request.Add("PAYGATE_ID", _payGateID);
            request.Add("PAY_REQUEST_ID", response["PAY_REQUEST_ID"]);
            request.Add("REFERENCE", Referrence);
            request.Add("CHECKSUM", _payment.GetMd5Hash(request, _payGateKey));

            string requestString = _payment.ToUrlEncodedString(request);

            StringContent content = new StringContent(requestString, Encoding.UTF8, "application/x-www-form-urlencoded");

            // ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            HttpResponseMessage res = await client.PostAsync("https://secure.paygate.co.za/payweb3/query.trans", content);
            res.EnsureSuccessStatusCode();

            string _responseContent = await res.Content.ReadAsStringAsync();

            Dictionary<string, string> results = _payment.ToDictionary(_responseContent);
            if (!results.Keys.Contains("ERROR"))
            {
                _payment.UpdateTransaction(results, results["PAY_REQUEST_ID"]);
            }

        }

        public async Task<ViewResult> Complete(int? id, string reference)
        {
            await _order.CreateOrder(reference);
            TempData["Status"] = id;
            TempData["Reference"] = reference;
       
            return View();
        }
    }
}