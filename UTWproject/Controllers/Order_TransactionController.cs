using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTWproject.Models;
using Newtonsoft.Json;
namespace UTWproject.Controllers
{
    public class Order_TransactionController : Controller
    {
        static DBentities db = new DBentities();
        int currentUserID = db.Users.Where(Z => Z.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().ID;
        // GET: Order_Transaction
        public ActionResult Index()
        {
            //List<Stock> stks = db.Stocks.ToList();

            //ViewBag.ListOfOrders = new SelectList(stks, "ID", "EnglishName");
            return View();
        }
        public JsonResult GetOrderList()
        {
            List<OrderTransactionModel> ordList = db.Orders.Where(x => x.UserID == currentUserID).Where(x => x.Date == DateTime.Today).Select(x => new OrderTransactionModel
            {
                ID = x.ID,
                Type = x.Type,
                Quantity = x.Quantity,
                UserID = x.UserID,
                StockID = x.StockID,
                Date = x.Date,
                StockName = x.Stock.EnglishName,
                StockPrice = x.Stock.Price
            }).ToList();
            return Json(ordList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOrderById(int ID)
        {
            Order ord = db.Orders.Where(x => x.ID == ID).Where(x => x.UserID == currentUserID).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(ord, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        class StockQuntity
        {
            public int quantity;
        }
        public JsonResult SaveDataInDatabase(OrderTransactionModel orderData)
        {
            var result = false;
            //Check If User try to sell quantity more than you have it
            if (orderData.Type == false)
            {
                List<StockQuntity> query1 = (from ord in db.Orders
                                             where ord.UserID == currentUserID && ord.StockID == orderData.StockID && ord.Type == true
                                             group ord by new { ord.UserID, ord.StockID, ord.Type } into o
                                             select new StockQuntity { quantity = o.Sum(x => x.Quantity) }).Take(1).ToList();
                int buy = 0;
                if (query1.Count > 0)
                {
                    buy = query1[0].quantity;
                }



                List<StockQuntity> query2 = (from ord in db.Orders
                                             where ord.UserID == currentUserID && ord.StockID == orderData.StockID && ord.Type == false
                                             group ord by new { ord.UserID, ord.StockID, ord.Type } into o
                                             select new StockQuntity { quantity = o.Sum(x => x.Quantity) }).Take(1).ToList();

                int sell = 0;
                if (query2.Count > 0)
                {
                    sell = query2[0].quantity;
                }
                int selledBefore = 0;
                if (orderData.ID > 0)
                {
                    selledBefore = db.Orders.Where(Z => Z.ID == orderData.ID).FirstOrDefault().Quantity;
                    selledBefore = orderData.Quantity - selledBefore;
                }

                if ((buy - sell) < orderData.Quantity && orderData.ID <= 0)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else if (((buy - sell) < selledBefore) && orderData.ID > 0)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            try
            {
                if (orderData.ID > 0)
                {
                    Order ord = db.Orders.SingleOrDefault(x => x.ID == orderData.ID);
                    ord.Quantity = orderData.Quantity;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    Order ord = new Order();
                    ord.UserID = currentUserID;
                    ord.Date = DateTime.Today;
                    ord.Quantity = orderData.Quantity;
                    ord.StockID = orderData.StockID;
                    ord.Type = orderData.Type;
                    db.Orders.Add(ord);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}