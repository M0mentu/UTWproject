//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UTWproject
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int ID { get; set; }
        public bool Type { get; set; }
        public int Quantity { get; set; }
        public int UserID { get; set; }
        public int StockID { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual Stock Stock { get; set; }
        public virtual User User { get; set; }
    }
}
