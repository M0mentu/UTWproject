using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTWproject.Models
{
    public class FilterOrdersModel
    {
        
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string  stockID { get; set; }
        public string  UserID { get; set; }
        public string username { get; set; }

    }
}