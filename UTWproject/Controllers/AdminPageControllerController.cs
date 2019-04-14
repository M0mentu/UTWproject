using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UTWproject.Controllers
{
    public class AdminPageController : Controller
    {
        DBentities db = new DBentities();
        // GET: AdminPage
        [Authorize]
        public ActionResult Index(string state = "All", string name = "", string email = "")
        {
            bool isAdmin = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Admin;
            if (isAdmin)
            {
                ViewBag.state = state;
                ViewBag.name = name;
                ViewBag.email = email;
                return View();
            }
            else
            {
                return RedirectToAction("NotAuthorized");
            }
        }
        [Authorize]
        public ActionResult NotAuthorized()
        {
            return View();
        }
        [Authorize]
        public ActionResult Disblock(int id)
        {
            User user = db.Users.Where(x => x.ID == id).FirstOrDefault();
            user.State = "Active";
            var entry = db.Entry(user);
            entry.Property(e => e.State).IsModified = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}