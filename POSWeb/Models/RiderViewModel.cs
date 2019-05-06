using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWeb.Models
{
    public class RiderViewModel
    {
        public int riderId { get; set; }
        public string riderUserName { get; set; }
        public string email { get; set; }
        public string riderPassword { get; set; }
        public string registration { get; set; }
        public string model { get; set; }
        public string color { get; set; }
        public string phone { get; set; }
        public string DeviceId { get; set; }
        public bool IsAvailable { get; set; }
    }
}