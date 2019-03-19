using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POSWeb.Controllers
{
    public class ProductsController : Controller
    {
        private posEntities db = new posEntities();
        // GET: Products
        public ActionResult Index()
        {
            var data = db.dbo_get_ProductsListForIndex().ToList();
            return View(data);
            //return View(db.Orders.ToList());
        }
    }
}