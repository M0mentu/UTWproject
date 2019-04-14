using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTWproject.Models;
namespace UTWproject.Controllers
{

    public class MonitorTransactionsController : Controller
    {
        DBentities DB = new DBentities();
        // GET: MonitorTransactions
        [Authorize]
        public ActionResult Index()
        {
            //---------------------------------------
          
            string username = (string)Session["username"];
            User u = DB.Users.Where(x => x.UserName == username).Where(x => x.Admin == true).FirstOrDefault();
            TempData["admin"] = u;
            List<Order> ords = new List<Order>();
            return View(ords);
        }

        [HttpPost]
        public ActionResult Index(FilterOrdersModel FOM)
        {

            List<Order> ords = new List<Order>();
            User u = DB.Users.Where(x => x.UserName == FOM.username).Where(x => x.Admin == true).FirstOrDefault();
            TempData["admin"] = u;
            if (FOM.stockID==null && FOM.fromDate == default(DateTime) && FOM.toDate == default(DateTime)&& FOM.UserID==null)
            {
                return View(ords);
            }
            else
            {
                    bool useradmin = false;
                    if (u!=null)
                    {
                        useradmin = true;
                    }
                User loginuser = DB.Users.Where(x => x.UserName == FOM.username).FirstOrDefault();
                int UID = loginuser.ID;
                //var query = from ord in DB.Orders where ((ord.Date >= FOM.fromDate && ord.Date <= FOM.toDate) || FOM.fromDate == default(DateTime) || FOM.toDate == default(DateTime)) && (ord.StockID == FOM.stockID || FOM.stockID == null) && (ord.UserID == FOM.UserID || FOM.UserID == null || useradmin==true) select ord;
                
                var query = from ord in DB.Orders where ((ord.Date >= FOM.fromDate && ord.Date <= FOM.toDate) || FOM.fromDate == default(DateTime) || FOM.toDate == default(DateTime)) && (ord.StockID.ToString() == FOM.stockID || FOM.stockID == null) && (ord.UserID.ToString() == FOM.UserID || (FOM.UserID == null && useradmin == true)|| (FOM.UserID == null && ord.UserID == UID)) select ord;

                ords = query.ToList();
                return View(ords);
                
                
            }
            
        }
    }
}