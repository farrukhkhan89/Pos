using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POSWeb.Controllers
{
    public class ReportsController : Controller
    {
        bravodeliver_posEntities db = new bravodeliver_posEntities();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult SalesReport(int days = 0)
        //{

        //    if (days != 0)
        //    {
        //        var data = db.report_sales_simple(days).ToList();
        //        return View(data);
        //    }
        //    else
        //    {
        //        var data = db.report_sales_simple(-10000).ToList();
        //        return View(data);
        //    }
        //}

        //public ActionResult CustomerReport()
        //{
        //        var data = db.report_customer_basic().ToList();
        //        return View(data);
         
        //}



    }
}