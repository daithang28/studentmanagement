using CourseWorkProject.Areas.Tutor.Data;
using CourseWorkProject.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseWorkProject.Areas.Staff.Controllers
{
    public class TutorDashboardController : Controller
    {
        private CWContext db = new CWContext();
        // GET: Staff/TutorDashboard
        public ActionResult Index()
        {
            var list = db.Users.Where(h => h.Role.id == 2).ToList();
            return View(list);
        }

        public ActionResult GetData(int id)
        {
            var CurrentTutor = db.Users.Where(h => h.Role.id == 2 && h.id == id).FirstOrDefault();
            var chatList = db.Chats.Where(h => h.ReceivePersonId == CurrentTutor.id && h.IsSent == true).ToList();
            var SendPerson = chatList.Select(h => h.SendPersonId).Distinct();
            var model = new List<DashTutorVM>();
            foreach (var item in SendPerson)
            {
                var thisStudent = db.Users.FirstOrDefault(h => h.id == item);
                DashTutorVM newDash = new DashTutorVM();
                newDash.Name = thisStudent.Profile.Name;
                newDash.MessageCount = chatList.Count(h => h.SendPersonId == item); ;
                model.Add(newDash);

                //Message.Add(chatList.Count(h => h.SendPersonId == item));
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}