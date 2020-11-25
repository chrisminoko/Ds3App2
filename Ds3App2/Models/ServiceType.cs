using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class ServiceType
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}