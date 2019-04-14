using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using UTWproject.Models;
using CaptchaMvc.HtmlHelpers;

namespace UTWproject.Controllers
{
    public class LoginAndLogoutController : Controller
    {
        DBentities DB = new DBentities();
        // GET: LoginAndLogout
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.captchacount = 0.ToString();
            return View();
        }
        [HttpPost]
        public ActionResult Login(loginModel model)
        {
          
            
            ViewBag.captchacount = 0.ToString();
           
            if(model.Username == null || model.Password == null){
                
            }
            else {
                Session["username"] = model.Username.ToString();
              
                string hashedPassword = LogInOut_ForgPass_Func.GenerateSHA256String(model.Password);
                if (DB.Users.Where(x => x.UserName == model.Username).Where(x => x.State != "Active").FirstOrDefault()!=null)
                {
                    ViewBag.message = "User Blocked";
                }
                else if(DB.Users.Where(x => x.UserName == model.Username).Where(x => x.IsEmailVerified == false).FirstOrDefault() != null)
                {
                    ViewBag.message = "User not verified";
                }
             
                else if (DB.Users.Where(x => x.UserName == model.Username).Where(x => x.Password == hashedPassword).FirstOrDefault() != null)
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                1, // Ticket version
                                model.Username, // Username to be associated with this ticket
                                DateTime.Now, // Date/time ticket was issued
                                DateTime.Now.AddDays(14), // Date and time the cookie will expire
                                false, // if user has chcked rememebr me then create persistent cookie
                                model.Username,
                                FormsAuthentication.FormsCookiePath);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cooki = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cooki.Expires = DateTime.Now.AddDays(14);
                    cooki.HttpOnly = true;
                    Response.Cookies.Add(cooki);

                    using (DBentities db = new DBentities())
                    {
                        LogInOut_ForgPass_Func HelperFunc = new LogInOut_ForgPass_Func();
                        User resetCaptcha = HelperFunc.UsernameExists(model.Username);
                        resetCaptcha.CaptchaCounter = 0;
                        db.Users.Attach(resetCaptcha);
                        var ourentry = db.Entry(resetCaptcha);
                        ourentry.Property(e => e.CaptchaCounter).IsModified = true;
                        db.SaveChanges();
                    }
                    return RedirectToAction("UserDashboard", "Dashboard");
                }
                else
                {
                    ViewBag.message = "Wrong password";

                    LogInOut_ForgPass_Func HelperFunc = new LogInOut_ForgPass_Func();
                    User LoginUser = HelperFunc.UsernameExists(model.Username);
                    //User LoginUser = (from u in DB.Users
                    //          where u.Email == model.EmailID
                    //          select u).FirstOrDefault();
                    if (LoginUser != null)
                    {
                       
                        LoginUser.CaptchaCounter++;
                        ViewBag.captchacount = LoginUser.CaptchaCounter.ToString();
                        DB.Users.Attach(LoginUser);
                        var entry = DB.Entry(LoginUser);
                        entry.Property(e => e.CaptchaCounter).IsModified = true;
                        // other changed properties
                        DB.SaveChanges();
                        if (LoginUser.CaptchaCounter > 3)
                        {
                            return RedirectToAction("Captcha");
                        }
                    }
                    else
                    {
                        
                        ViewBag.message = "Wrong username or password";
                    }
                }
                
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
        public ActionResult Captcha()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Captcha(string empty)
        {
            LogInOut_ForgPass_Func HelperFunc = new LogInOut_ForgPass_Func();
            string username = (string)Session["username"];
            User LoginUser = HelperFunc.UsernameExists(username);
            if (this.IsCaptchaValid("Captcha is not valid"))
            { 
                LoginUser.CaptchaCounter = 0;
                DB.Users.Attach(LoginUser);
                var entry1 = DB.Entry(LoginUser);
                entry1.Property(e => e.CaptchaCounter).IsModified = true;
                // other changed properties
                DB.SaveChanges();
                return RedirectToAction("Login");
            }
            LoginUser.CaptchaCounter++;
            DB.Users.Attach(LoginUser);
            var entry2 = DB.Entry(LoginUser);
            entry2.Property(e => e.CaptchaCounter).IsModified = true;
            // other changed properties
            DB.SaveChanges();
            if (LoginUser.CaptchaCounter > 5)
            {
                LoginUser.State = "Blocked";
                var entry3 = DB.Entry(LoginUser);
                entry3.Property(e => e.State).IsModified = true;
                DB.SaveChanges();
                return RedirectToAction("BlockUser");
            }
            
            ViewBag.ErrMessage = "Error: captcha is not valid.";
            return View();
        }
        public ActionResult BlockUser()
        {
            return View();
        }
    }
}