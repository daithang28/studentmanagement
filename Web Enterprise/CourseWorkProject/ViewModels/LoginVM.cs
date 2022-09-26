using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseWorkProject.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Invalid User name")]
        [Display(Name ="User name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Invalid Password")]
        [Display(Name = "Password")]
        public string Pass { get; set; }
    }
}