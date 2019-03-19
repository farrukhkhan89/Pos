using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWeb.Models
{
    public class OrderViewModel
    {
       public List<Order> orderObj { get; set; }
        public List<OrderDetail> orderDetails { get; set; }


    }
}