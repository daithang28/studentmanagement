using CourseWorkProject.Areas.Staff.Data;
using CourseWorkProject.DAL;
using CourseWorkProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Common;

namespace CourseWorkProject.Areas.Staff.Controllers
{
   [Authorize(Roles ="Staff")]
    public class MainController : Controller
    {
        // GET: Staff/Home
        private CWContext db = new CWContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Allocate()
        {


            var StudentList = db.Users.ToList();
            return View(StudentList);
        }

        [HttpPost]
        public ActionResult Allocate(int[] check, string Id)
        {

            string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/template/Mailtemplate.html"));
            var list = db.Users.ToList();
            var LectId = int.Parse(Id);
            //Lấy thông tin group của lecture
            var myLect = db.Groups.Where(h => h.id == LectId).FirstOrDefault();
            string LectName = myLect.User.Profile.Name;
            content = content.Replace("{{TutorName}}", LectName);
            //kiểm tra danh sách học sinh gửi vào có rỗng hay k?
            if (check == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Nếu ko rỗng
            if (myLect != null)
            {
                //Lấy thông tin danh sách member của group do 1 Lecture đảm nhận
                var myGroupmember = db.GroupMembers.Where(h => h.Group.id == myLect.id).FirstOrDefault();

                //nếu chưa có member
                if (myGroupmember == null)
                {
                    foreach(var mem in check)
                    {
                        if (isAllocated(mem))
                        {
                            var user = db.Users.Where(h => h.id == mem).FirstOrDefault();
                            GroupMember member = new GroupMember();
                            member.Group = myLect;
                            member.idStudent = mem;
                            
                            content = content.Replace("{{StudentName}}", user.Profile.Name);
                            content = content.Replace("{{TutorName}}", LectName);
                            new MailHelper().SendMail(user.Profile.Email, "Allocate Information", content);
                            new MailHelper().SendMail(myLect.User.Profile.Email, "Allocate Information", content);
                            db.GroupMembers.Add(member);
                            user.GroupMember = member;
                            db.SaveChanges();
                        }
                        else
                        {
                            var user = db.Users.Where(h => h.id == mem).FirstOrDefault();
                            var myStudent = db.GroupMembers.Where(h => h.idStudent == mem).FirstOrDefault();
                            myStudent.Group = myLect;
                            user.GroupMember = myStudent;
                            content = content.Replace("{{StudentName}}", user.Profile.Name);
                            content = content.Replace("{{TutorName}}", LectName);
                            new MailHelper().SendMail(user.Profile.Email, "Allocate Information", content);
                            new MailHelper().SendMail(myLect.User.Profile.Email, "Allocate Information", content);
                            db.SaveChanges();
                        }
                                            
                    }
                }

                else if(myGroupmember != null)
                {
                    foreach (var mem in check)
                    {
                        if (isDuplicate(mem, myLect))
                        {
                            if (isAllocated(mem))
                            {
                                var user = db.Users.Where(h => h.id == mem).FirstOrDefault();
                                GroupMember newMember = new GroupMember();
                                newMember.idStudent = mem;                               
                                newMember.Group = myLect;
                                db.GroupMembers.Add(newMember);
                                user.GroupMember = newMember;
                                content = content.Replace("{{StudentName}}", user.Profile.Name);
                                content = content.Replace("{{TutorName}}", LectName);
                                new MailHelper().SendMail(user.Profile.Email, "Allocate Information", content);
                                new MailHelper().SendMail(myLect.User.Profile.Email, "Allocate Information", content);
                                db.SaveChanges();
                            }
                            else
                            {
                                var user = db.Users.Where(h => h.id == mem).FirstOrDefault();
                                var myStudent = db.GroupMembers.Where(h => h.idStudent == mem).FirstOrDefault();
                                myStudent.Group = myLect;
                                user.GroupMember = myStudent;
                                content = content.Replace("{{StudentName}}", user.Profile.Name);
                                content = content.Replace("{{TutorName}}", LectName);
                                new MailHelper().SendMail(user.Profile.Email, "Allocate Information", content);
                                new MailHelper().SendMail(myLect.User.Profile.Email, "Allocate Information", content);
                                db.SaveChanges();
                            }

                        }
                    }
                }

            }

            return View(list);
        }

        public bool isDuplicate(int idStu, Group current)
        {
            var check = db.GroupMembers.Where(h => h.idStudent == idStu && h.Group.id == current.id).FirstOrDefault();
            if (check == null)
                return true;
            return false;

        }

        public bool isAllocated(int id)
        {
            var student = db.GroupMembers.Where(h => h.idStudent == id).FirstOrDefault();
            if (student == null)
                return true;
            return false;
           
        }
    }
}