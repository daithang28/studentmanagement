using CourseWorkProject.DAL;
using CourseWorkProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseWorkProject.Controllers
{
    [Authorize(Roles ="Student")]
    public class DashboardController : Controller
    {
        private CWContext db = new CWContext();
        // GET: Dashboard     
        public ActionResult Index()
        {
          
            return View();
        }
        public ActionResult LoadDashboard(string user)
        {
            var currentUser = db.Users.Where(h => h.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
            var UserGroup = db.GroupMembers.Where(h => h.idStudent == currentUser.id).FirstOrDefault();
            var groupMem = db.GroupMembers.Where(h => h.idStudent == currentUser.id).FirstOrDefault();
            if (groupMem == null)
            {
                return RedirectToAction("Fail", "Dashboard");
            }

            var PersonalLecture = db.Users.FirstOrDefault(h => h.id == UserGroup.Group.id);
            var chatList = db.Chats.Where(h => h.SendPersonId == currentUser.id && h.ReceivePersonId == PersonalLecture.id && h.IsSent == true).ToList();
            var dateOnly = chatList.Select(h => h.CreatedDate).Distinct();
            List<string> dateString = new List<string>();
            List<int> Message = new List<int>();
            foreach (var item in dateOnly)
            {
                Message.Add(chatList.Count(h => h.CreatedDate== item));
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