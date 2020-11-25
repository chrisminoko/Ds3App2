using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ds3App2.Models
{
    public class Booking
    {
        public Guid Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string ServiceType { get; set; }
        public string Comment { get; set; }
        public string Reference { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; }
        public DateTime DateTimeStamp { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Mileage { get; set; }
        public bool IsComplete { get; set; }
        public string CustomerId { get; set; }
        public string PhoneNumber { get; set; }
        public bool CheckIn { get; set; }
        public bool CheckOut { get; set; }
        public bool HasJobCard { get; set; }
        
        //ApplicationDbContext db = new ApplicationDbContext();

        //public string getContact()
        //{
        //    var Contact = (from s in db.Customer
        //                 where s.Id == Id
        //                 select s.Contact).FirstOrDefault();
        //    return Contact;
        //}

        //public string getFName()
        //{
        //    var FNAme = (from s in db.Customer
        //                   where s.Id == Id
        //                   select s.FirstName).FirstOrDefault();
        //    return FNAme;
        //}

        //public string getLName()
        //{
        //    var LName = (from s in db.Customer
        //                   where s.Id == Id
        //                   select s.LastName).FirstOrDefault();
        //    return LName;
        //}
    }
}