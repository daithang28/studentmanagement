using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseWorkProject.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public virtual Role Role { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual Group Group { get; set; }
        public virtual GroupMember GroupMember { get; set; }
        //public ICollection<Blog> Blogs { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public virtual ChatBox ChatBox { get; set; }
        public ICollection<Sender> Senders { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Blog> Blogs { get; set; }
    }
}