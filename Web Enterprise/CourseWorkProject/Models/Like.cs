using CourseWorkProject.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseWorkProject.Models
{
    public class Like
    {
        public int id { get; set; }
        public LikeStatus Status { get; set; }
        public virtual User User { get; set; }
        public virtual Blog Blog { get; set; }
    }
}