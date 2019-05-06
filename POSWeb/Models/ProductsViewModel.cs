using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWeb.Models
{
    public class ProductsViewModel
    {/*
         "Products" : [
    {
      "quantity" : 1,
      "title" : "ALKEMIST 750ML",
      "productId" : "040232521386",
      "price" : "$39.99"
    }





        */
        public string productId { get; set; }
        public string title { get; set; }
        public int quantity { get; set; }
        public string price { get; set; }
    }
}