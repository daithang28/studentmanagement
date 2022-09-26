using CourseWorkProject.DAL;
using CourseWorkProject.Models;
using CourseWorkProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseWorkProject.Areas.Tutor.Controllers
{
    [Authorize(Roles ="Tutor")]
    public class TutorChatController : Controller
    {
        // GET: Tutor/TutorChat
        CWContext db = new CWContext();
        public ActionResult Index()
        {
            var CurrentName = HttpContext.User.Identity.Name;
            var CurrentUser = db.Users.FirstOrDefault(h => h.UserName == CurrentName);
          
            var GroupList = db.Groups.Where(h => h.id == CurrentUser.Group.id).FirstOrDefault();
            var myList = db.Users.Where(h => h.UserName != CurrentName && h.GroupMember.Group.id == GroupList.id).ToList();
         
               return View(myList);
      

        }

        [HttpGet]
        public JsonResult ListChat(string friendId)
        {
            int myAccountId = int.Parse(friendId);
            var CurrentName = db.Users.FirstOrDefault(h => h.UserName == HttpContext.User.Identity.Name);
            User acc = db.Users.SingleOrDefault(m => m.id == CurrentName.id);              
            ChatBox friendChatBox = db.ChatBoxes.FirstOrDefault(t => t.User.id == myAccountId);
            var myListChat = (from c in db.Chats
                              where c.ChatBox.ChatBoxId == friendChatBox.ChatBoxId
                              && c.SendPersonId == acc.id
                              select new
                              {
                                  chatContent = c.Content,
                                  createdDate = c.CreatedDate,
                                  isSent = c.IsSent,                                  
                             }).ToList();
           
            return Json(myListChat, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SaveChat(string accountId, string chatContent)
        {
            var CurrentName = db.Users.FirstOrDefault(h => h.UserName == HttpContext.User.Identity.Name);
            try
            {
                int friendAccountId = int.Parse(accountId);
                ChatBox chatBoxFriend = db.ChatBoxes.FirstOrDefault(c => c.User.id == friendAccountId);
                ChatBox myChatBox = db.ChatBoxes.FirstOrDefault(c => c.User.id == CurrentName.id);
                DateTime current = DateTime.Now;
                Chat newChat = new Chat()
                {
                    SendPersonId = CurrentName.id,
                    ReceivePersonId = friendAccountId,
                    ChatBox = chatBoxFriend,
                    Content = chatContent,
                    IsSent = true,
                    CreatedDate = current.Date
                };

                Chat newChat2 = new Chat()
                {
                    SendPersonId = friendAccountId,
                    ReceivePersonId   = CurrentName.id,
                    ChatBox = myChatBox,
                    Content = chatContent,
                    CreatedDate = current.Date
                };
                db.Chats.Add(newChat);
                db.Chats.Add(newChat2);
                db.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("", "This message cannot save.");
            }
            return null;
        }
    }
}