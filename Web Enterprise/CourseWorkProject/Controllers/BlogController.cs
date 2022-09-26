using Common;
using CourseWorkProject.Constant;
using CourseWorkProject.DAL;
using CourseWorkProject.Models;
using CourseWorkProject.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseWorkProject.Controllers
{
    [Authorize(Roles = "Student")]
    public class BlogController : Controller
    {
        private CWContext db = new CWContext();
        // GET: Blog
        public ActionResult Index()
        {

            var currentUser = db.Users.FirstOrDefault(h => h.UserName == HttpContext.User.Identity.Name);
            ViewBag.Empty = "There are no blogs";
            var groupMem = db.GroupMembers.Where(h => h.idStudent == currentUser.id).FirstOrDefault();
            if (groupMem == null)
            {
                return RedirectToAction("Fail", "Blog");
            }
            var BlogList = db.Blogs.Include("Likes").Include("Category")
                .Where(h => h.Group.GroupName == currentUser.GroupMember.Group.GroupName).OrderByDescending(h => h.CreateDate).ToList();
            List<Blog> list = new List<Blog>();
            foreach (var item in BlogList)
            {
                if (item.Content == null) continue;
                int MaxLength = item.Content.Length;
                int Length = (MaxLength / 2);
                item.Content = item.Content.Substring(0, Length) + "...";
                list.Add(item);
            }

            ViewBag.currentUser = currentUser;
            isEvent(DateTime.Now);
            return View(list);
        }

        public ActionResult CreateBlog()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBlog(BlogVM model, HttpPostedFileBase File)
        {
            var myPath = "";
            if (File != null && File.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(File.FileName);
                myPath = _FileName;
                string _Path = Path.Combine(Server.MapPath("~/UploadFiles"), _FileName);
                File.SaveAs(_Path);
                ViewBag.Message = "File Uploaded Successfully!!";
            }
            if (ModelState.IsValid)
            {
                var CurrentUser = db.Users.Where(h => h.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
                if (CurrentUser != null)
                {
                    model.Creator = CurrentUser.Profile.Name;
                    model.FileName = myPath;
                    Blog newBlog = new Blog();
                    newBlog.Content = model.Content;
                    newBlog.CreateDate = model.CreateDate;
                    newBlog.FileName = model.FileName;
                    if (CurrentUser.id == 2)
                    {
                        newBlog.Group = CurrentUser.Group;
                    }
                    else
                    {
                        newBlog.Group = CurrentUser.GroupMember.Group;
                    }
                    newBlog.Title = model.Title;
                    newBlog.Creator = model.Creator;
                    db.Blogs.Add(newBlog);
                    db.SaveChanges();
                }

            }
            return RedirectToAction("Index", "Blog");
        }

        public ActionResult Like(int id, LikeStatus status)
        {
            var CurrentUser = db.Users.Where(h => h.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
            var blog = db.Blogs.FirstOrDefault(h => h.id == id);
            var like = db.Likes.Include("Blog").Include("User").FirstOrDefault(x => x.Blog.id == id && x.User.id == CurrentUser.id);

            if (like == null)
            {
                like = new Like { 
                    Blog = blog,
                    User = CurrentUser,
                    Status = status
                };
                db.Likes.Add(like);
            }
            else
            {
                if (like.Status != status)
                    like.Status = status;
                else
                    like.Status = LikeStatus.Empty;
            }
            db.SaveChanges();

            var blogNew = db.Blogs.Include("Likes").FirstOrDefault(h => h.id == id);


            return Json(new {
                id = blogNew.id,
                like = blogNew.Likes.Count(x => x.Status == LikeStatus.Like),
                disLike = blogNew.Likes.Count(x => x.Status == LikeStatus.Dislike),
            }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetComments(int blogid)
        {
            var BlogComments = db.Comments.Where(h => h.Blog.id == blogid).ToList();
            return PartialView("~/Views/Shared/_Comment.cshtml", BlogComments);
        }

        public ActionResult AddComment(CommentVM comment, int blogId)
        {
            if (comment.Content != null)
            {
                var UserComment = new Comment();
                var CurrentUser = db.Users.Where(h => h.UserName == comment.Creator).FirstOrDefault();
                var CurrentBlog = db.Blogs.Include("CreatedBy").FirstOrDefault(h => h.id == blogId);
                if (CurrentUser != null)
                {
                    comment.Creator = CurrentUser.Profile.Name;
                }
                UserComment.Content = comment.Content;
                UserComment.CreateDate = DateTime.Now;
                UserComment.User = CurrentUser;
                UserComment.Blog = CurrentBlog;
                db.Comments.Add(UserComment);
                db.SaveChanges();

                sendNotification(CurrentBlog.CreatedBy.Profile.Name, 
                    CurrentBlog.CreatedBy.Profile.Email, 
                    "There is a new comment in your Idea", 
                    string.Format("Have a new comment in your '{0}' Idea. <br /> Content: '{1}'", 
                        CurrentBlog.Title,
                        UserComment.Content)
                    );
            }
            return Json(comment, JsonRequestBehavior.AllowGet);
        }

        void sendNotification(string userName, string userEmail, string title, string contentText)
        {

            var special = DateTime.Now.ToString();
            special = special.Substring(0, 10);
            string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/template/NotificationEmailTemplate.html"));
            content = content.Replace("{{user}}", userName);
            content = content.Replace("{{title}}", title);
            content = content.Replace("{{date}}", special);
            content = content.Replace("{{eventContent}}", contentText);
            new MailHelper().SendMail(userEmail, title, content);

        }

        bool isEvent(DateTime today)
        {
            var currentUser = db.Users.FirstOrDefault(h => h.UserName == HttpContext.User.Identity.Name);
            var eventDate = db.Events.FirstOrDefault(h => h.EventDate.Day == today.Day &&
                                                     h.EventDate.Month == today.Month);
            if (eventDate != null)
            {
                if (isSend(currentUser.UserName, eventDate.id))
                {
                    var checkSended = new Sender();
                    checkSended.isSend = true;
                    checkSended.Event = eventDate;
                    checkSended.User = currentUser;
                    checkSended.year = today.Year;
                    db.Senders.Add(checkSended);
                    db.SaveChanges();

                    var special = today.ToString();
                    special = special.Substring(0, 10);
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/template/EventEmailTemplate.html"));
                    content = content.Replace("{{user}}", currentUser.Profile.Name);
                    content = content.Replace("{{title}}", eventDate.Title);
                    content = content.Replace("{{date}}", special);
                    content = content.Replace("{{eventContent}}", eventDate.Content);
                    new MailHelper().SendMail(currentUser.Profile.Email, "Special Event", content);
                    return true;
                }

            }

            return false;
        }

        bool isSend(string Name, int id)
        {
            var currentUser = db.Users.FirstOrDefault(h => h.UserName == HttpContext.User.Identity.Name);
            var sended = db.Senders.SingleOrDefault(h => h.User.UserName == Name && h.year == DateTime.Now.Year && h.id == id);
            if (sended == null)
            {
                return true;
            }
            return false;
        }
        public ActionResult Fail()
        {
            return View();
        }

        public ActionResult BlogDetail(int Id)
        {
            BlogComVM model = new BlogComVM();
            var myBlog = db.Blogs.Where(h => h.id == Id).FirstOrDefault();
            var myComment = db.Comments.Where(h => h.Blog.id == Id).ToList();
            model.Blog = myBlog;
            model.Comments = myComment;
            return View(model);

        }
    }
}
     