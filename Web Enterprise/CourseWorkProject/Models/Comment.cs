using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseWorkProject.Models
{
    public class Comment
    {
        public int id { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual User User { get; set; }
        public virtual Blog Blog { get; set; }

        public Comment()
        {
            CreateDate = DateTime.Now;
        }
    }
}