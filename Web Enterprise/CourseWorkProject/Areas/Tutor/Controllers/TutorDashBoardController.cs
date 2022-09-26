using CourseWorkProject.Areas.Tutor.Data;
using CourseWorkProject.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseWorkProject.Areas.Tutor.Controllers
{
    [Authorize(Roles ="Tutor")]
    public class TutorDashBoardController : Controller
    {
        private CWContext db = new CWContext();
        // GET: Tutor/TutorDashBoard
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult DashboardData(string user, string SearchString, string SortType)
        {
            var CurrentUser = db.Users.FirstOrDefault(h => h.UserName == user);
            
            var chatList = db.Chats.Where(h => h.ReceivePersonId == CurrentUser.id && h.IsSent == true).ToList();
            var SendPerson = chatList.Select(h => h.SendPersonId).Distinct();
            var model = new List<DashTutorVM>();
            #region 
            //List<int> Message = new List<int>();
            //List<string> SendUser = new List<string>();  
            #endregion
            foreach (var item in SendPerson)
            {
                var thisStudent = db.Users.FirstOrDefault(h => h.id == item);
                DashTutorVM newDash = new DashTutorVM();
                newDash.Name = thisStudent.Profile.Name;
                newDash.MessageCount = chatList.Count(h => h.SendPersonId == item); ;
                model.Add(newDash);

                //Message.Add(chatList.Count(h => h.SendPersonId == item));
            }
            #region
            //int isAdded = 0;
            //foreach (var item in chatList)
            //{

            //    var senderID = db.Users.FirstOrDefault(h => h.id == item.SendPersonId);

            //    if (senderID.id != isAdded)
            //    {
            //        SendUser.Add(senderID.Profile.Name);
            //        isAdded = senderID.id;
            //    }

            //}
            //DashTutorVM model = new DashTutorVM();
            //model.MessageCount = Message.ToList();
            //model.Name = SendUser;
            #endregion

            if (!String.IsNullOrEmpty(SearchString))
            {
                model = model.Where(h => h.Name.ToUpper().Contains(SearchString.ToUpper())).ToList();
            }
            switch (SortType)
            {
                case "Count": model = model.OrderByDescending(h => h.MessageCount).ToList();
                    break;
                case "Name":
                    model = model.OrderByDescending(h => h.Name).ToList();
                    break;
                default:
                    model = model.ToList();
                    break;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


    }
}