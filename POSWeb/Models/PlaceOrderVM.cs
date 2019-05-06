using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWeb.Models
{
    public class PlaceOrderVM
    {
        public string userId { get; set; }
        public string scheduleDateTime { get; set; }
        public string grandTotal { get; set; }
        public string orderDateTime { get; set; }
        public string email { get; set; }
        public string nonce { get; set; }
        public List<ProductsViewModel> Products { get; set; }
    }

    public class ProductsViewModels
    {
        public string quantity { get; set; }
        public string title { get; set; }
        public string productId { get; set; }
        public string price { get; set; }
    }

}