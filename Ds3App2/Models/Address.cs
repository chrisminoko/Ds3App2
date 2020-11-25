using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Street Number")]
        public string StreetNumber { get; set; }
        [Required]
        [Display(Name = "Street Name")]
        public string StreetName { get; set; }
        [Required]
        [Display(Name = "Surburb")]
        public string Surburb { get; set; }
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        public int PostalCode { get; set; }
        public string CustomerId { get; set; }
        public bool IsDeleted { get; set; }
    }
}