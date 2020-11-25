using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class JobTask
    {
        public Guid Id { get; set; }
        [Display(Name = "Job")]
        public string Job { get; set; }
        [Display(Name = "Service Type")]
        public Guid ServiceType { get; set; }
        [Display(Name = "Charge (ZAR)")]
        public decimal RatePerHour { get; set; }
        public bool IsDeleted { get; set; }
    }
}