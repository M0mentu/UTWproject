using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTWproject.Models;


namespace UTWproject.Controllers
{
    public class OrderTransactionController : Controller
    {
     static  DBentities db = new DBentities();
        int currentUserID = db.Users.Where(Z => Z.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().ID;

        // GET: OrderTransaction
        [Authorize]
        public ActionResult Transaction()
        {
            var orders = db.Orders.Where(x => x.UserID == currentUserID).Where(x=>x.Date==DateTime.Today).ToList();
            return View(orders);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Transaction(Order orderData)
        {
            
            return View();
        }

        [Authorize]
        public ActionResult addEdit()
        {
                return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult addEdit(Order orderData)
        {
            string message = "";

            orderData.UserID = currentUserID;
            orderData.Date = DateTime.Today;
            db.Orders.Add(orderData);
            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Transaction", "OrderTransaction");
            }
            else
            {
                message = "Invalid Request";

            }
            return RedirectToAction("Transaction");

        }

        [Authorize]
        [HttpGet]
        public ActionResult editOrder(int id)
        {
            var orderE = db.Orders.Find(id);
            if (orderE != null)
            {
                return View(orderE);
            }
            return RedirectToAction("Transaction", "OrderTransaction");
        }

        [Authorize]
        [HttpPost]
        public ActionResult editOrder(Order orderData)
        {
            orderData.UserID = currentUserID;
            orderData.Date = DateTime.Today;
            //  db.Entry(orderData).State = System.Data.Entity.EntityState.Modified ;
            db.Set<Order>().AddOrUpdate(orderData);

            db.SaveChanges();
            return RedirectToAction("Transaction", "OrderTransaction");
        }


    }
}