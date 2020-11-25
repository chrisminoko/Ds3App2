using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Product Description")]
        public string ProductDescription { get; set; }
        [Required]
        [Display(Name = "Brand")]
        public string Brand { get; set; }
        [Required]
        public decimal Price { get; set; }

        [Display(Name = "Stock Quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int StockQuantity { get; set; }
        [Display(Name = "Product Image")]
        public string ProductImage { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        [Display(Name = "Stock Low Level")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int LowLevel { get; set; }
        public Product()
        {
            Id = Guid.NewGuid();
        }
    }
}