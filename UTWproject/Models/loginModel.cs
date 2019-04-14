using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UTWproject.Models
{
    public class loginModel
    {
        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username required")]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "password required ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

    }
}