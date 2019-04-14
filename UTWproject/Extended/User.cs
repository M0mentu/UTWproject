
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UTWproject
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        

    }
    public class UserMetadata
    {
        [Display(Name = "Englis Name")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "only letters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "English Name is required")]
        public string EnglishName { get; set; }

        [Display(Name = "Arabic Name")]
      [RegularExpression("^[\u0621-\u064A ]+$", ErrorMessage = "only arabic letters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Arabic Name is required")]
        public string ArabicName { get; set; }


        [Display(Name = "UserName")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "No symbols allowed")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "username is required")]
        public string UserName { get; set; }

        [Display(Name = "Email ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }



        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string Password { get; set; }


        [Display(Name = "Answer")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "No symbols allowed")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Question Answer Name is required")]
        public string QuestionAnswer { get; set; }

       

    }
}
