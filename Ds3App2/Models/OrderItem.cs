using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Reference { get; set; }
        public bool IsDeleted { get; set; }
    }
}