using CourseWorkProject.DAL;
using CourseWorkProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseWorkProject.Areas.Staff.Controllers
{
    [Authorize(Roles ="Staff")]
    public class StudentDashboardController : Controller
    {
        private CWContext db = new CWContext();
        // GET: Staff/StudentDashboard
        public ActionResult Index()
        {
            var list = db.Users.Where(h => h.Role.id == 1).ToList();
            return View(list);
        }
        public ActionResult GetData(int id)
        {
            var student = db.Users.FirstOrDefault(h => h.id == id);

            var UserGroup = db.GroupMembers.Where(h => h.idStudent == student.id).FirstOrDefault();
            var groupMem = db.GroupMembers.Where(h => h.idStudent == student.id).FirstOrDefault();
            if (groupMem == null)
            {
                return RedirectToAction("Fail", "StudentDashboard");
            }

            var PersonalLecture = db.Users.FirstOrDefault(h => h.id == UserGroup.Group.id);
            var chatList = db.Chats.Where(h => h.SendPersonId == student.id && h.ReceivePersonId == PersonalLecture.id && h.IsSent == true).ToList();
            var dateOnly = chatList.Select(h => h.CreatedDate).Distinct();
            List<string> dateString = new List<string>();
            List<int> Message = new List<int>();
            foreach (var item in dateOnly)
            {
                Message.Add(chatList.Count(h => h.CreatedDate == item));
            }
            var isAdded = "";
            foreach (var item in chatList)
            {

                var date = item.CreatedDate.ToString();
                date = date.Substring(0, 10);
                if (date != isAdded)
                {
                    dateString.Add(date);
                    isAdded = date;
                }

            }
            DashVM model = new DashVM();
            model.MessageCreateDate = dateString;
            model.MessageCount = Message.ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Fail()
        {
            return View();
        }
    }
}