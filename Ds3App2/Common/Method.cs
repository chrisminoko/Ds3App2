using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Ds3App2.Common
{
    public class Method
    {
        private readonly string welcome = "Welcome to Car Services and Repaires";
        private readonly string welcomeBody = "Thank you for joining our family and we promise to take of your vehicle needs and wants.";
        private async Task SendEmail(string HTMLTemplate, MailAddress emailAddress, string Subject)
        {
            var apiKey = ConfigurationManager.AppSettings["apikey"];
            var fromName = ConfigurationManager.AppSettings["sendGridUser"];
            var fromEmail = ConfigurationManager.AppSettings["sendGridFrom"];

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, fromName);
            var subject = Subject;
            var to = new EmailAddress(emailAddress.Address, "");
            var plainTextContent = "";
            var htmlContent = HTMLTemplate;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            await client.SendEmailAsync(msg);
        }

        public async Task Welcome(string email)
        {
            string Body = string.Empty;

            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/Welcome.html")))
            {
                Body = reader.ReadToEnd();
            }
            Body = Body.Replace("{Header}", welcome);
            Body = Body.Replace("{Message}", welcomeBody);

            await SendEmail(Body, new MailAddress(email), welcome);

        }

    }
}