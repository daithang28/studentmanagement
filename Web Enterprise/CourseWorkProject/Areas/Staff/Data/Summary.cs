using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseWorkProject.Areas.Staff.Data
{
    public class Summary
    {
        public List<ReportVM> Report { get; set; }
        public List<LectMessageCountVM> Lecture { get; set; }
    }
}