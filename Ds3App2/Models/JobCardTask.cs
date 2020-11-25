using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class JobCardTask
    {
        public Guid Id { get; set; }
        public Guid JobCard { get; set; }
        public Guid JobTask { get; set; }
        public string ServiceType { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAdded { get; set; }
        public bool IsCompleted { get; set; }
        public Guid BookingId { get; set; }
    }
}