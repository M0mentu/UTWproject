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
    public class RegistrationController : Controller
    {
        DBentities DB = new DBentities();
        LogInOut_ForgPass_Func RegisterHelperFunc = new LogInOut_ForgPass_Func();
        // GET: Registration
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (DBentities db = new DBentities())
            {
                db.Configuration.ValidateOnSaveEnabled = false;//to avoid confirm password does not match problem on save

                var v = db.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }

            }
            ViewBag.Status = Status;
            return View();

        }
        public ActionResult Register()
        {
            ViewBag.questions = DB.Questions.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Exclude = "Admin,State,IsEmailVerified,ActivationCode")]User user)
        {

            bool Status = false;
            string message = "";

            //
            //model validation
            if (ModelState.IsValid)
            {

                #region    //email already exists
                //User exist = RegisterHelperFunc.UserExists(user.Email);
                User emailExist = DB.Users.Where(x => x.Email == user.Email).FirstOrDefault();
                User usernameExist = RegisterHelperFunc.UsernameExists(user.UserName);
                if (emailExist != null && usernameExist != null)
                {
                    ViewBag.validate = "Username and Email already exist";
                    return View(user);
                }
                else if (emailExist != null)
                {
                    ViewBag.validate = "Email already exist";
                    ModelState.AddModelError("Email Exists", "Email already exists");
                    return View(user);
                }
                else if (usernameExist != null)
                {
                    ViewBag.validate = "Username already exist";
                    return View(user);
                }
                #endregion
                #region Generate Activation code
                user.ActivationCode = Guid.NewGuid();
                #endregion
                #region password hashing
                user.Password = LogInOut_ForgPass_Func.GenerateSHA256String(user.Password);
                //user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                
                #region answer hashing
                user.QuestionAnswer = LogInOut_ForgPass_Func.GenerateSHA256String(user.QuestionAnswer);
                #endregion

                user.IsEmailVerified = false;
                user.CaptchaCounter = 0;
                user.navigateToLink = false;
                #region save to database
                // using (DBentities db = new DBentities())
                //{
                //   user.State = "active";
                ///  string ques = Request.Params[6].ToString();
                // user.Question = (from q in db.Questions
                //                 where q.QTextEnglish == ques
                //                select q).FirstOrDefault();
                user.Admin = false;
                user.State = "Active";
                DB.Users.Add(user);
                DB.SaveChanges();

                //send email to user
                RegisterHelperFunc.OldURL = Request.Url.PathAndQuery;
                RegisterHelperFunc.sendverification(user.Email, user.ActivationCode.ToString());
                message = "successfully registered. Account activation link has been sent to email: " + user.Email;
                Status = true;
                // }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            //generate activation code

            //password  hashing 


            //save data to database
            ViewBag.Message = message;
            ViewBag.Status = Status;
            ViewBag.questions = DB.Questions.ToList();
            return View(user);
            //return Content("ok");
        }
    }
}