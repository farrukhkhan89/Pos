using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWeb.Models
{
    public class StoreViewModel
    {
        public string storeName { get;  set; }
        public int id { get; set; }
        public string imageUrl { get; set; }

        public string ZipCode { get;  set; }
        public string Address { get;  set; }
        public string Lat { get;  set; }
        public string City { get;  set; }
        public string Lng { get;  set; }
        public string phoneNumber { get;  set; }
        public string email { get; set; }
        public string State { get; set; }
    }
}