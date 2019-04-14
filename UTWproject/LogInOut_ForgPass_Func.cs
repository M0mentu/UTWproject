using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using UTWproject.Models;
using CaptchaMvc.HtmlHelpers;

using System.Configuration;
using System.Net.Configuration;

namespace UTWproject.Controllers
{
    public class LogInOut_ForgPass_Func : Controller
    {
        public string OldURL;
        [NonAction]
        public User UpdateNavToLisk(string id)
        {
            using (DBentities DB = new DBentities())
            {
                User returnedUser = (from u in DB.Users
                                     where u.ResetPasswordCode.ToString() == id
                                     select u).FirstOrDefault();
                return returnedUser;
            }
            
        }
        [NonAction]
        public User UsernameExists(string username)
        {
            using (DBentities db = new DBentities())
            {
                User returnedUser = (from u in db.Users
                                  where u.UserName == username
                                     select u).FirstOrDefault();
                //User v = db.Users.Where(a => a.Email == emailID).FirstOrDefault();
                return returnedUser;
            }

        }
        
        [NonAction]
        public void sendverification(string email, string activationCode, string emailFor = "Registration/VerifyAccount")
        {
            //var verifyUrl = "Home" + "/" + emailFor + "/" + activationCode;
            //var link = Request.Url.AbsoluteUri.Replace(OldURL, verifyUrl);
            var link = "http://localhost:5178/"  + emailFor + "/" + activationCode; 
             var fromEmail = new MailAddress(ConfigurationManager.AppSettings["smtpUser"], "UTW10");//emailhere
            var toEmail = new MailAddress(email);
            //var frompassword = "Utw10Utw10";//password here
            string subject = "";
            string body = "";
            if (emailFor == "Registration/VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "Forget_Password/ResetPassword")
            {

                subject = "Reset Password";
                body = "Hi,<br/>br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                //Credentials = new NetworkCredential(fromEmail.Address, frompassword)
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["smtpUser"], ConfigurationManager.AppSettings["smtpPass"])
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);

        }
        [NonAction]
        public static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }
        [NonAction]
        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }


    }
}