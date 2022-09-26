using CourseWorkProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseWorkProject.ViewModels
{
    public class CategoryVM
    {
        public CategoryVM()
        {
            CreateDate = DateTime.Now;
        }
        public int id { get; set; }
        public string Creator { get; set; }
        [Required(ErrorMessage = "please input your Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreateDate { get; set; }
        public ICollection<Blog> Blogs { get; set; }
    }
}