using Ds3App2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ds3App2.ViewModels
{
    public class InvoiceViewModel
    {
        public Invoice Invoice { get; set; }
        public Address Address { get; set; }
        public Vehicle Vehicle { get; set; }
        public Customer Customer { get; set; }
    }
}