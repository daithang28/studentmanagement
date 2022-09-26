using CourseWorkProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseWorkProject.Areas.Staff.Data
{
    public class ExceptionVM
    {
        public List<User> StdWithoutTutor { get; set; }
        public List<User> StdWithoutInteract { get; set; }
    }
}