using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWeb.Models
{
    public class CustomerViewModel
    {
        public string Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string resetCode { get; set; }
        public string phone { get; set; }
        public string notification_playerId { get; set; }
        public int add_new_id { get; set; }
        public string shipping_state { get; set; }
        public string shipping_city { get; set; }
        public string billing_address1 { get; set; }
        public string billing_address2 { get; set; }
        public string billing_state { get; set; }
        public string billing_city { get; set; }
        public string billing_zipcode { get; set; }
        public string shipping_address1 { get; set; }
        public string shipping_address2 { get; set; }
        public string shipping_zipcode { get; set; }
    }
}