using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWeb.Models
{
    public class StoreProductsViewModel
    {
        public int ProdId { get; set; }
        public string Korona_ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public string StoreName { get; set; }

    }
}