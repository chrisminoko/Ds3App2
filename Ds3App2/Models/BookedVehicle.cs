using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class BookedVehicle
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public Guid VehilcleId { get; set; }
        public bool IsComplete { get; set; }
    }
}