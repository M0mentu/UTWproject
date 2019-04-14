using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTWproject.Models
{
    public class resetPasswordModel
    {
        public int id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Password { get; set; }
    }
}