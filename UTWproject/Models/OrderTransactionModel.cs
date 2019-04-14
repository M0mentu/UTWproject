using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UTWproject.Models
{
    public class OrderTransactionModel
    {
        public int ID { get; set; }
        public bool Type { get; set; }

        public int Quantity { get; set; }

        public int UserID { get; set; }
        public int StockID { get; set; }
        public DateTime Date { get; set; }
        public string StockName { get; set; }
        public double StockPrice { get; set; }
    }
}