using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        [Display(Name = "VIN")]
        public string Colour { get; set; }
        [Required]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }
        public string CustomerId { get; set; }
        public bool IsDeleted { get; set; }
    }
}