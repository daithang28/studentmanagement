using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseWorkProject.Models
{
    public class Sender
    {
        public Sender()
        {
            year = DateTime.Now.Year;
        }
        public int id { get; set; }
        public int year { get; set; }
        public bool isSend { get; set; }
        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
    }
}