using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Ds3App2.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public int TotalMaterial { get; set; }
        public decimal CostOfMaterial { get; set; }
        public int TotalTasks { get; set; }
        [Display(Name = "CostOfService")]
        public decimal CostOfLabour { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string CustomerId { get; set; }
        public bool Paid { get; set; }
        public DateTime DateTimeStamp { get; set; }
    }
}