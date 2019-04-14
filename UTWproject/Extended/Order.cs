using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UTWproject
{
    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
    }
    public class OrderMetadata
    {
        [Display(Name = "Order Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Type required")]
        public int Type { get; set; }

        [Display(Name = "Quantity")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Quantity is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Quantity must be numeric")]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Date { get; set; }
    }
}