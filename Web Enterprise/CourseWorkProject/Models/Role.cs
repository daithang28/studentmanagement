using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseWorkProject.Models
{
    public class Role
    {
        public int id { get; set; }
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}