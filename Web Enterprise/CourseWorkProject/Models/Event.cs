using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseWorkProject.Models
{
    public class Event
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime EventDate { get; set; }
        public ICollection<Sender> Senders { get; set; }
    }
}