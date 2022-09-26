using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseWorkProject.Models
{
    public class Group
    {
        [ForeignKey("User")]
        public int id { get; set; }
        public string GroupName { get; set; }
        public int idTutor { get; set; }

        public ICollection<GroupMember> GroupMembers { get; set; }
        public ICollection<Blog> Blogs { get; set; }
        public virtual User User { get; set; }
    }
}