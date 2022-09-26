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

namespace CourseWorkProject.Areas.Staff.Controllers
{
    [Authorize(Roles = "Staff")]
    public class TutorBlogController : Controller
    {

        // GET: Tutor/Blog   
        private CWContext db = new CWContext();
        // GET: Blog
        public ActionResult Index()
        {
            var currentUser = db.Users.Include("GroupMember").Include("Group").FirstOrDefault(h => h.UserName == HttpContext.User.Identity.Name);
            isEvent(DateTime.Now);
            ViewBag.Empty = "There are no blogs";
            ViewBag.currentUser = currentUser;
            if (currentUser.Role.id == 2)
            {
                var mylist = db.Blogs.Include("Likes").Include("Category").Where(h => h.Group.GroupName == currentUser.Group.GroupName).OrderByDescending(h => h.CreateDate).ToList();
                List<Blog> list = new List<Blog>();
                foreach (var item in mylist)
                {
                    if (item.Content == null) continue;
                    int MaxLength = item.Content.Length;
                    int Length = (MaxLength / 2);
                    item.Content = item.Content.Substring(0, Length) + "...";
                    list.Add(item);
                }

                isEvent(DateTime.Now);
                return View(list);

            }
            var BlogList = new List<Blog>();
            if (currentUser.GroupMember == null)
                BlogList = db.Blogs.Include("Likes").Include("Category").OrderByDescending(h => h.CreateDate).ToList();
            else
                BlogList = db.Blogs.Include("Likes").Include("Category").Where(h => h.Group.GroupName == currentUser.GroupMember.Group.GroupName).OrderByDescending(h => h.CreateDate).ToList();

            return View(BlogList);
        }

        public ActionResult TutorCreateBlog()
        {
            var categories = db.Categories.OrderByDescending(h => h.CreateDate).ToList();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public ActionResult TutorCreateBlog(BlogVM model, HttpPostedFileBase File)
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
                var CurrentUser = db.Users.Include("GroupMember").Include("Group").FirstOrDefault(h => h.UserName == HttpContext.User.Identity.Name);
                if (CurrentUser != null)
                {
                    var category = db.Categories.FirstOrDefault(x => x.id == model.Category);

                    model.Creator = CurrentUser.Profile.Name;
                    model.FileName = myPath;
                    Blog newBlog = new Blog();
                    newBlog.Content = model.Content;
                    newBlog.Category = category;
                    newBlog.CreateDate = model.CreateDate;
                    newBlog.FileName = model.FileName;
                    newBlog.CreatedBy = CurrentUser;
                    if (CurrentUser.Role.id == 2)
                    {
                        newBlog.Group = CurrentUser.Group;
                    }
                    else
                    {
                        if (CurrentUser.GroupMember != null)
                            newBlog.Group = CurrentUser.GroupMember.Group;
                    }
                    newBlog.Title = model.Title;
                    newBlog.Creator = model.Creator;
                    db.Blogs.Add(newBlog);
                    db.SaveChanges();

                    sendNotification(CurrentUser.Profile.Name, CurrentUser.Profile.Email, "Create Idea Success", string.Format("Create Idea '{0}' Success", newBlog.Title));
                }

            }
            return RedirectToAction("Index", "TutorBlog");
        }

        public ActionResult TutorUpdateBlog(int Id)
        {
            var categories = db.Categories.OrderByDescending(h => h.CreateDate).ToList();
            ViewBag.Categories = categories;

            var blog = db.Blogs.Include("Category").FirstOrDefault(h => h.id == Id);
            if (blog == null)
            {
                return RedirectToAction("Index", "TutorBlog");
            }
            return View(new BlogVM
            {
                id = blog.id,
                Category = blog.Category.id,
                Title = blog.Title,
                Content = blog.Content,
            });
        }

        [HttpPost]
        public ActionResult TutorUpdateBlog(BlogVM model)
        {
            if (ModelState.IsValid)
            {
                var CurrentUser = db.Users.Include("GroupMember").Include("Group").FirstOrDefault(h => h.UserName == HttpContext.User.Identity.Name);
                if (CurrentUser != null)
                {
                    var category = db.Categories.FirstOrDefault(x => x.id == model.Category);
                    var blog = db.Blogs.FirstOrDefault(h => h.id == model.id);
                    blog.Content = model.Content;
                    blog.Category = category;
                    blog.Title = model.Title;
                    db.SaveChanges();
                }

            }
            return RedirectToAction("Index", "TutorBlog");
        }

        public ActionResult Like(int id, LikeStatus status)
        {
            var CurrentUser = db.Users.Include("GroupMember").Include("Group").FirstOrDefault(h => h.UserName == HttpContext.User.Identity.Name);
            var blog = db.Blogs.FirstOrDefault(h => h.id == id);
            var like = db.Likes.Include("Blog").Include("User").FirstOrDefault(x => x.Blog.id == id && x.User.id == CurrentUser.id);

            if (like == null)
            {
                like = new Like
                {
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


            return Json(new
            {
                id = blogNew.id,
                like = blogNew.Likes.Count(x => x.Status == LikeStatus.Like),
                disLike = blogNew.Likes.Count(x => x.Status == LikeStatus.Dislike),
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int Id)
        {
            var blog = db.Blogs.Include("Comments").Include("Likes").FirstOrDefault(h => h.id == Id);

            if (blog != null)
            {
                db.Comments.RemoveRange(blog.Comments);
                db.Likes.RemoveRange(blog.Likes);
                db.Blogs.Remove(blog);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "TutorBlog");
        }

        public PartialViewResult GetComments(int blogid)
        {
            var BlogComments = db.Comments.Where(h => h.Blog.id == blogid).ToList();
            return PartialView("~/Views/Shared/_Comment.cshtml", BlogComments);
        }

        public ActionResult AddComment(CommentVM comment, int blogId)
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
            return Json(comment, JsonRequestBehavior.AllowGet);
        }

        bool isEvent(DateTime today)
        {
            var currentUser = db.Users.Include("GroupMember").Include("Group").FirstOrDefault(h => h.UserName == HttpContext.User.Identity.Name);
            var eventDate = db.Events.FirstOrDefault(h => h.EventDate.Day == today.Day &&
                                                     h.EventDate.Month == today.Month &&
                                                     h.EventDate.Year == today.Year);
            if (eventDate != null)
            {
                if (isSend(currentUser.UserName))
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

        bool isSend(string Name)
        {
            var currentUser = db.Users.FirstOrDefault(h => h.UserName == HttpContext.User.Identity.Name);
            var sended = db.Senders.SingleOrDefault(h => h.User.UserName == Name && h.year == DateTime.Now.Year);
            if (sended == null)
            {
                return true;
            }
            return false;
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