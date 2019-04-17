using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWeb.Models
{
    public class ProductsPlaceOrderModel
    {
        public int id { get; set; }
        public int quantity { get; set; }
        public decimal amount { get; set; }
    }
}