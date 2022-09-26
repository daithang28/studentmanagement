using CourseWorkProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseWorkProject.ViewModels
{
    public class BlogVM
    {
        public BlogVM()
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
        public virtual Group Group { get; set; }
        public int Category { get; set; }


    }
}