using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsDelivery { get; set; }
        public string Status { get; set; }
        public string StatusTraking { get; set; }
        public decimal Amount { get; set; }
        public bool IsDeleted { get; set; }
    }
}