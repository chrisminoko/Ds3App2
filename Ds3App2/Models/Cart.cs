using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public string Reference { get; set; }
        public Guid ProductId { get; set; }
        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}