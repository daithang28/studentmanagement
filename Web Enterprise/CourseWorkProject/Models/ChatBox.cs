using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseWorkProject.Models
{
    public class ChatBox
    {
        [ForeignKey("User")]
        public int ChatBoxId { get; set; }
        public string Name { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
    }
}