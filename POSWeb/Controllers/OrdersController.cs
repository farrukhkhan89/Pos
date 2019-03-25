using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POSWeb;

namespace POSWeb.Controllers
{
    //public class OrdersController : Controller
    //{
    //    private posEntities db = new posEntities();

    //    // GET: Orders
    //    public ActionResult Index()
    //    {
             
    //        var data = db.get_OrdersListForIndex().ToList();
    //        return View(data);
    //        //return View(db.Orders.ToList());
    //    }

    //    // GET: Orders/Details/5
    //    public ActionResult Details(string id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        Order order = db.Orders.Find(id);
    //        if (order == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(order);
    //    }

    //    // GET: Orders/Create
    //    public ActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: Orders/Create
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Create([Bind(Include = "OrderId,UserId,Total,ShippingAddress,City,State,Country,Zipcode,Phone,OrderType,CreatedDatetime,Shipped,trackingNo,StoreId")] Order order)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            db.Orders.Add(order);
    //            db.SaveChanges();
    //            return RedirectToAction("Index");
    //        }

    //        return View(order);
    //    }

    //    // GET: Orders/Edit/5
    //    public ActionResult Edit(string id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        Order order = db.Orders.Find(id);
    //        if (order == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(order);
    //    }

    //    // POST: Orders/Edit/5
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Edit([Bind(Include = "OrderId,UserId,Total,ShippingAddress,City,State,Country,Zipcode,Phone,OrderType,CreatedDatetime,Shipped,trackingNo,StoreId")] Order order)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            db.Entry(order).State = System.Data.Entity.EntityState.Modified;
    //            db.SaveChanges();
    //            return RedirectToAction("Index");
    //        }
    //        return View(order);
    //    }

    //    // GET: Orders/Delete/5
    //    public ActionResult Delete(string id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        Order order = db.Orders.Find(id);
    //        if (order == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(order);
    //    }

    //    // POST: Orders/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult DeleteConfirmed(string id)
    //    {
    //        Order order = db.Orders.Find(id);
    //        db.Orders.Remove(order);
    //        db.SaveChanges();
    //        return RedirectToAction("Index");
    //    }


 
    //    public ActionResult CancelOrder(string id)
    //    {
    //        Order order = db.Orders.Find(id);
    //        order.status = false;
    //        order.Cancelled = true;

    //        order.CancelledDateTime = DateTime.Now;

    //        TempData["msg"] = "Order has been cancled";
    //        db.SaveChanges();
    //        return RedirectToAction("Edit", new { id = id });
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }
    //}
}
