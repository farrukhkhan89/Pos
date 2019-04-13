using POSWeb.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace POSWeb.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        posEntities2 db = new posEntities2();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Dashboard()
        {
            //ViewBag.Message = "Your contact page.";
            var currentDate = DateTime.Now.Date;


            ViewBag.totalSales = db.Orders.Where(x => x.Cancelled == null || x.Cancelled == false).Sum(x => x.Total);
            ViewBag.TodaySales = db.Orders.Where(x => (x.Cancelled == null || x.Cancelled == false) && DbFunctions.TruncateTime(x.CreatedDatetime) == currentDate).Sum(x => x.Total);

            ViewBag.OrdersToday = db.Orders.Where(x => (x.Cancelled == null || x.Cancelled == false) && DbFunctions.TruncateTime(x.CreatedDatetime) == currentDate).Count();
            ViewBag.TotalDelivered = db.Orders.Where(x => (x.Cancelled == null || x.Cancelled == false) && x.deliveryDate != null).Count();


            var test = db.Orders.Count();

            return View();
        }
    }
}