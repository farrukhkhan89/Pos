using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWeb.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string orderDateTime { get; set; }
        public string email { get; set; }
        public string scheduleDateTime { get; set; }
        public string riderUserName { get; set; }
        public string riderPhone { get; set; }

       


    }
}