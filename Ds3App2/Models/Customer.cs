using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
    }
}