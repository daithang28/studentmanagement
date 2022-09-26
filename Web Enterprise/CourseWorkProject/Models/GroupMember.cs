using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseWorkProject.Models
{
    public class GroupMember
    {
        public int id { get; set; }
        public int idStudent { get; set; }
        public virtual Group Group { get; set; }
        public ICollection<User> Users { get; set; }
    }
}