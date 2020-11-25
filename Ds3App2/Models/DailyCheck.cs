using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ds3App2.Models
{
    public class DailyCheck
    {
        public Guid Id { get; set; }
        public DateTime DateChecked { get; set; }
        public bool Checked { get; set; }
        public DailyCheck()
        {
            Id = Guid.NewGuid();
            DateChecked = DateTime.Today;
        }
    }
}