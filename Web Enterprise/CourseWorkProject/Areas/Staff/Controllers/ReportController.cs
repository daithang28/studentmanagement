using CourseWorkProject.Areas.Staff.Data;
using CourseWorkProject.DAL;
using CourseWorkProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseWorkProject.Areas.Staff.Controllers
{
    [Authorize(Roles ="Staff")]
    public class ReportController : Controller
    {
        // GET: Staff/Report
        private CWContext db = new CWContext();
        public ActionResult Index()
        {
            ExceptionVM model = new ExceptionVM();
            var Student = db.Users.Where(h => h.Role.id == 1).ToList();
            var studentWithoutTutor = db.Users.Where(h => h.Role.id == 1 && h.GroupMember == null).ToList();
            model.StdWithoutTutor = studentWithoutTutor;
            var chatList = db.Chats.Select(h => h.SendPersonId).Distinct().ToList();
            var n = new List<int>();
            var stdList = new List<User>();
            foreach (var item in Student)
            {
                if (!chatList.Contains(item.id))
                {
                   n.Add(item.id);
                }
            }
            foreach (var item in n)
            {
                var std = db.Users.Where(h => h.id == item).FirstOrDefault();
                stdList.Add(std);
            }
            model.StdWithoutInteract = stdList.ToList();
            return View(model);
        }
        public JsonResult DashboarData()
        {
            var NumberMessage = db.Chats.Where(h => h.IsSent == true).ToList();
            var Dateonly = NumberMessage.Select(h => h.CreatedDate).Distinct();
            var Lecture = db.Users.Where(h => h.Role.id == 2).ToList();
           // var MesageToLect = db.Chats.Where(h)
            var MessageLast7Days = new List<ReportVM>();
            foreach (var item in Dateonly)
            {
                var date = item.ToString();
                date = date.Substring(0, 10);
                ReportVM model = new ReportVM();
                model.DateString = date;
                model.MessageCount = NumberMessage.Count(h => h.CreatedDate == item);
                MessageLast7Days.Add(model);
            }
            var LectCount = new List<LectMessageCountVM>();
            foreach (var item in Lecture)
            {
                var MessageToLect = db.Chats.Where(h => h.ReceivePersonId == item.id && h.IsSent == true).ToList();
                LectMessageCountVM model = new LectMessageCountVM();
                model.Name = item.Profile.Name;
                model.Count = MessageToLect.Count(h => h.ReceivePersonId == item.id);
                LectCount.Add(model);
            }
            Summary list = new Summary();
            list.Report = MessageLast7Days;
            list.Lecture = LectCount;
            return Json(list,JsonRequestBehavior.AllowGet);
        }
    }
}