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
    
    public partial class LiveTracking
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string RiderId { get; set; }
        public string CustomerId { get; set; }
        public string LatLng { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<bool> TrackingComplated { get; set; }
    }
}
