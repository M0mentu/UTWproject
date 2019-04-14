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
    public class Forget_PasswordController : Controller
    {
        DBentities DB = new DBentities();
        LogInOut_ForgPass_Func RegisterHelperFunc = new LogInOut_ForgPass_Func();
        // GET: ForgetPassword
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(string id)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";
            bool status = false;

            var account = DB.Users.Where(a => a.Email == id).FirstOrDefault();
            if (account != null)
            {
                //Send email for reset password
                Guid resetCode = Guid.NewGuid();
                //RegisterHelperFunc.OldURL = Request.Url.PathAndQuery;
                /*LogInOut_ForgPass_Func HelperFunc = new LogInOut_ForgPass_Func();
                User returneUser = HelperFunc.UserExists(id);
                returneUser.navigateToLink = false;
                DB.Users.Attach(returneUser);
                var entry = DB.Entry(returneUser);
                entry.Property(e => e.navigateToLink).IsModified = true;
                // other changed properties
                DB.SaveChanges();*/
                RegisterHelperFunc.sendverification(account.Email, resetCode.ToString(), "Forget_Password/ResetPassword");
                account.ResetPasswordCode = resetCode;
                account.ResetPasswordDate = DateTime.Now;
                account.navigateToLink = false;
                //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                //in our model class in part 1
                DB.Configuration.ValidateOnSaveEnabled = false;
                DB.SaveChanges();
                message = "Reset password link has been sent to your email id.";
            }
            else
            {
                message = "Account not found";
            }

            ViewBag.Message = message;
            //return RedirectToAction("Login", "LoginAndLogout");
            return View();
        }
        public ActionResult ResetPassword(string id)
        {
            List<Question> QList = DB.Questions.ToList();
            ViewBag.Quests = QList;
            LogInOut_ForgPass_Func HelperFunc = new LogInOut_ForgPass_Func();
            User returnedUser = HelperFunc.UpdateNavToLisk(id);
            if (returnedUser == null)
            {
                return RedirectToAction("Expired");
            }
            int userID = returnedUser.ID;
            DateTime epiredDate = (DateTime)(returnedUser.ResetPasswordDate);
            bool navToLink = returnedUser.navigateToLink;
            //int userID = DB.Users.Where(x => x.ResetPasswordCode.ToString() == id).FirstOrDefault().ID;
            //DateTime epiredDate = (DateTime)(DB.Users.Where(x => x.ID == userID).FirstOrDefault().ResetPasswordDate);
            //bool navToLink = DB.Users.Where(x => x.ID == userID).FirstOrDefault().navigateToLink;
            if (DateTime.Now > epiredDate.AddDays(1) || navToLink == true)
            {
                return RedirectToAction("Expired");
            }
            returnedUser.navigateToLink = true;
            DB.Users.Attach(returnedUser);
            var entry = DB.Entry(returnedUser);
            entry.Property(e => e.navigateToLink).IsModified = true;
            // other changed properties
            DB.SaveChanges();
            resetPasswordModel model = new resetPasswordModel();
            model.id = userID;
            return View(model);
        }
        [HttpPost]
        public ActionResult ResetPassword(resetPasswordModel model)
        {
            User U = DB.Users.Where(n => n.ID == model.id).Where(n => n.ID == model.id).FirstOrDefault();
            string hashedAnswer = LogInOut_ForgPass_Func.GenerateSHA256String(model.Answer);
            int questinID = DB.Questions.Where(a => a.QTextEnglish == model.Question).FirstOrDefault().ID;
            //User x = DB.Users.Where(n => n.ID == model.id).Where(n => n.QuestionAnswer == hashedAnswer).FirstOrDefault();
            if (questinID == U.QuestionID && U.QuestionAnswer == hashedAnswer)
            {
                U.Password = LogInOut_ForgPass_Func.GenerateSHA256String(model.Password);
                TempData["passwordChanged"] = "Password changed successfully";
                DB.Entry(U).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
            }
            else
            {
                TempData["passwordChanged"] = "Question Or answer is wrong";
            }
            return RedirectToAction("PassChanged");
        }
        public ActionResult Expired()
        {
            return View();
        }
        public ActionResult PassChanged()
        {
            return View();
        }
    }
}