using CourseWorkProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseWorkProject.ViewModels
{
    public class UserVM
    {
        public List<User> Student { get; set; }
        public int LectId { get; set; }
        public SelectList mySelect { get; set; }
    }
}