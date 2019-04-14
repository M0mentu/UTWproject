using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTWproject.Models
{
    public class forgetPasswordModel
    {
        public int questionID { get; set; }
        public string questionAnswer { get; set; }
        public string Email { get; set; }
    }
}