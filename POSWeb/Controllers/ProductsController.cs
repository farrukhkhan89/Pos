using POSWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POSWeb.Controllers
{
    public class ProductsController : Controller
    {
        private posEntities2 db = new posEntities2();
        // GET: Products
        public ActionResult Index()
        {
            var storeProds = (from t1 in db.Products
                              join t2 in db.Stores on t1.StoreId equals t2.id
                              
                              select new StoreProductsViewModel
                              {
                                  ProdId = t1.Id,
                                  Korona_ProductId = t1.Korona_ProductId,
                                  Name = t1.Name,
                                  Image = t1.Image,
                                  Price = t1.Price,
                                  Category = t1.Category
                              }).OrderBy(x=>x.Name).ToList();

            return View(storeProds);
        }
    }
}