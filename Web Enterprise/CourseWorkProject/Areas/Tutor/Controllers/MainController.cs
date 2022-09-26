using CourseWorkProject.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CourseWorkProject.Areas.Tutor.Controllers
{
    [Authorize(Roles = "Tutor")]
    public class MainController : Controller
    {
        // GET: Tutor/Main
        private CWContext db = new CWContext();
        public ActionResult Index()
        {
            var myInfo = db.Users.Where(h => h.UserName.Equals(HttpContext.User.Identity.Name)).FirstOrDefault();
            return View(myInfo);
        }

        public ActionResult ListStudent()
        {
            var name = HttpContext.User.Identity.Name;
            var Currentlect = db.Users.Where(h => h.UserName == name).FirstOrDefault();
            if (Currentlect == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            var list = db.Users.Where(h => h.GroupMember.Group.id == Currentlect.id).ToList();
            return View(list);

        }
    }
}