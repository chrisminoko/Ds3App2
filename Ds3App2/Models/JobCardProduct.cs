using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class JobCardProduct
    {
        public Guid Id { get; set; }
        public Guid JobCard { get; set; }
        public Guid JobProduct { get; set; }
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAdded { get; set; }
        public bool IsCompleted { get; set; }
        public Guid BookingId { get; set; }
    }
}