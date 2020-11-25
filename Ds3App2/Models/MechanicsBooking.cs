using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class MechanicsBooking
    {
        [Key]
        public Guid MechanicsBookingId { get; set; }
        public string MechanicId { get; set; }
        public DateTime DateBooked { get; set; }
        public Guid BookingId { get; set; }
    }
}