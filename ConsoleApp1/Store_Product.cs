//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleApp1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Store_Product
    {
        public int Store_Product1 { get; set; }
        public Nullable<int> StoreId { get; set; }
        public string ProductId { get; set; }
    
        public virtual Store Store { get; set; }
        public virtual Product Product { get; set; }
    }
}
