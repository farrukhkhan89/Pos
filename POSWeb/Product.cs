//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace POSWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Store_Product = new HashSet<Store_Product>();
        }
    
        public int Id { get; set; }
        public string Korona_ProductId { get; set; }
        public string Korona_ProductNumber { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public Nullable<int> Stock { get; set; }
        public string Category { get; set; }
        public Nullable<bool> BelongToStore { get; set; }
        public string Price { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Store_Product> Store_Product { get; set; }
    }
}
