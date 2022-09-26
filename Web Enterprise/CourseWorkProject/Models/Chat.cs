using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseWorkProject.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public string Content { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }
        public bool? IsSent { get; set; }
        public int SendPersonId { get; set; }
        public int ReceivePersonId { get; set; }
        public virtual ChatBox ChatBox { get; set; }
    }
}