using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseWorkProject.ViewModels
{
    public class CommentVM
    {
        public string Creator { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
    }
}