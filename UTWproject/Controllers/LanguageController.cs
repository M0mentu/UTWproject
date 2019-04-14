using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace UTWproject.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult Change(string language,string url)
        {
            if (language != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
            }

            HttpCookie cookie = new HttpCookie("language");
            cookie.Value = language;
            Response.Cookies.Add(cookie);

            return Redirect(url);
        }
    }
}