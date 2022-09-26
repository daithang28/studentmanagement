using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseWorkProject.Models
{
    public class Profile
    {
        [ForeignKey("User")]
        public int id { get; set; }
        public string Name { get; set; }
        public string img { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
        public int idUser { get; set; }
        public virtual User User { get; set; }
    }
}