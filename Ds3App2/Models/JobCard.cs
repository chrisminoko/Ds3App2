using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class JobCard
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public string Mechanic { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string BookingIdString { get; set; }
    }
}