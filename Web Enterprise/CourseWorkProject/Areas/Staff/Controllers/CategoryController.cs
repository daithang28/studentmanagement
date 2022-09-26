using Common;
using CourseWorkProject.DAL;
using CourseWorkProject.Models;
using CourseWorkProject.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace CourseWorkProject.Areas.Staff.Controllers
{
    [Authorize(Roles = "Staff")]
    public class CategoryController : Controller
    {
        private CWContext db = new CWContext();
        // GET: Category
        public ActionResult Index()
        {
            ViewBag.Empty = "There are no categories";
            var categories = db.Categories.Include("Blogs").OrderByDescending(h => h.CreateDate).ToList();
            isEvent(DateTime.Now);
            return View(categories);
        }

        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(CategoryVM model)
        {
            if (ModelState.IsValid)
            {
                var CurrentUser = db.Users.Where(h => h.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
                if (CurrentUser != null)
                {
                    model.Creator = CurrentUser.Profile.Name;
                    Category newCategory = new Category();
                    newCategory.Name = model.Name;
                    newCategory.CreateDate = model.CreateDate;
                    newCategory.Creator = model.Creator;
                    db.Categories.Add(newCategory);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Category");
        }

        public ActionResult UpdateCategory(int Id)
        {
            var category = db.Categories.FirstOrDefault(h => h.id == Id);
            return View(category);
        }

        [HttpPost]
        public ActionResult UpdateCategory(CategoryVM model)
        {
            if (ModelState.IsValid)
            {
                var CurrentUser = db.Users.Where(h => h.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
                if (CurrentUser != null)
                {
                    var category = db.Categories.FirstOrDefault(h => h.id == model.id);
                    category.Name = model.Name;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Category");
        }

        public ActionResult Delete(int Id)
        {
            var category = db.Categories.Include("Blogs").FirstOrDefault(h => h.id == Id);

            if (!category.Blogs.Any())
            {
                db.Categories.Remove(category);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Category");
        }

        bool isEvent(DateTime today)
        {
            var currentUser = db.Users.FirstOrDefault(h => h.UserName == HttpContext.User.Identity.Name);
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
    }
}