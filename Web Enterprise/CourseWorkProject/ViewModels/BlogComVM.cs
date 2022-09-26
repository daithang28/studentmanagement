using CourseWorkProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseWorkProject.ViewModels
{
    public class BlogComVM
    {
        public Blog Blog { get; set; }
        public List<Comment> Comments { get; set; }
    }
}