using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Customer Surname")]
        public string CustomerSurname { get; set; }
        [Display(Name = "Customer Email")]
        public string CustomerEmail { get; set; }
        [Display(Name = "Type")]
        public string OrderType { get; set; }
        [Display(Name = "Reference")]
        public string OrderReference { get; set; }
        [Display(Name = "Amount(ZAR)")]
        public string Amount { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        public bool IsVerified { get; set; }
        public DateTime PaymentDate { get; set; }
        public Payment()
        {
            Id = Guid.NewGuid();
            Status = "Not Verified";
            IsVerified = false;
            PaymentDate = DateTime.Now;
        }

    }
}