using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace CourseWorkProject.Models
{
    public class Blog
    {

        public Blog()
        {
            CreateDate = DateTime.Now;
        }
        public int id { get; set; }
        public string Creator { get; set; }
        [Required(ErrorMessage = "please input your Title")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        public string Content { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreateDate { get; set; }
        public string FileName { get; set; }
        //public virtual User User { get; set; }
        public Group Group { get; set; }
        public Category Category { get; set; }
        public User CreatedBy { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }





    }
}