using CourseWorkProject.DAL;
using CourseWorkProject.Models;
using CourseWorkProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseWorkProject.Controllers
{
    [Authorize(Roles ="Student")]
    public class ChatController : Controller
    {
        // GET: Chat

        CWContext db = new CWContext();
        public ActionResult Index()
        {
            var CurrentName = HttpContext.User.Identity.Name;
            var CurrentUser = db.Users.FirstOrDefault(h => h.UserName == CurrentName);
            var groupMem = db.GroupMembers.Where(h => h.idStudent == CurrentUser.id).FirstOrDefault();
            if (groupMem == null)
            {
                return RedirectToAction("Fail", "Chat");
            }
            var GroupMem = db.GroupMembers.Where(h => h.Group.id == CurrentUser.GroupMember.Group.id).FirstOrDefault();
            var list = db.Users.Where(h => h.UserName != CurrentName && h.GroupMember.Group.id == GroupMem.Group.id || h.UserName != CurrentName && h.Group.id == GroupMem.Group.id).ToList();
            return View(list);
        }

        [HttpGet]
        public JsonResult ListChat(string friendId)
        {
            var CurrentName = db.Users.FirstOrDefault(h=> h.UserName== HttpContext.User.Identity.Name);
            User acc = db.Users.SingleOrDefault(m => m.id == CurrentName.id);
            int myAccountId = int.Parse(friendId);
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
            ViewBag.myAvatar = CurrentName.Profile.img;
            var myFriend = db.Users.FirstOrDefault(h => h.id == myAccountId);
            ViewBag.friendAvatar = myFriend.Profile.img;
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
                    CreatedDate = current.Date,
                    IsSent = true
                };

                Chat newChat2 = new Chat()
                {
                    SendPersonId = friendAccountId,
                    ReceivePersonId = CurrentName.id,
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
        public ActionResult Fail()
        {
            return View();
        }
    }
}
