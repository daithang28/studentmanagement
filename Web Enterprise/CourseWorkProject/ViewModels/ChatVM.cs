using CourseWorkProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseWorkProject.ViewModels
{
    public class ChatVM
    {
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}