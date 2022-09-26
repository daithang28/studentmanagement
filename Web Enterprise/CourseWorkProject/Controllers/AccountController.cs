using CourseWorkProject.DAL;
using CourseWorkProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CourseWorkProject.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        CWContext db = new CWContext();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                if (isValid(model))
                {
                    FormsAuthentication.SetAuthCookie(model.Name, false);
                    var role = Roles.GetRolesForUser(model.Name);
                    if (role.Contains("Student"))
                    {
                        return RedirectToAction("Index", "Blog");
                    }
                    else if (role.Contains("Tutor"))
                    {
                        return RedirectToAction("Index", "TutorBlog", new { area = "Tutor" });
                    }
                    else if (role.Contains("Staff"))
                    {
                        return RedirectToAction("Allocate", "Main", new { area = "Staff" });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Account");
                }
            }
            return View(model);
        }

        public bool isValid(LoginVM model)
        {
          var account = db.Users.Where(h => h.UserName.Equals(model.Name) &&
          h.Password.Equals(model.Pass)).FirstOrDefault();
            if (account != null)
                return true;
            return false;
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}