using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UTWproject.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize]
        public ActionResult UserDashboard()
        {
            return View();
        }
    }
}