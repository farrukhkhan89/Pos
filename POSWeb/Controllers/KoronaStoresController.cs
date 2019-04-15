using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POSWeb;

namespace POSWeb.Controllers
{
    public class KoronaStoresController : Controller
    {
        private bravodeliver_posEntities db = new bravodeliver_posEntities();

        // GET: KoronaStores
        public ActionResult Index()
        {
            return View(db.KoronaStores.ToList());
        }

        // GET: KoronaStores/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KoronaStore koronaStore = db.KoronaStores.Find(id);
            if (koronaStore == null)
            {
                return HttpNotFound();
            }
            return View(koronaStore);
        }

        // GET: KoronaStores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KoronaStores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sno,Korona_StoreId,storeName,Address,City,State,ZipCode,phoneNumber,email")] KoronaStore koronaStore)
        {
            if (ModelState.IsValid)
            {
                db.KoronaStores.Add(koronaStore);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(koronaStore);
        }

        // GET: KoronaStores/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KoronaStore koronaStore = db.KoronaStores.Find(id);
            if (koronaStore == null)
            {
                return HttpNotFound();
            }
            return View(koronaStore);
        }

        // POST: KoronaStores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sno,Korona_StoreId,storeName,Address,City,State,ZipCode,phoneNumber,email")] KoronaStore koronaStore)
        {
            if (ModelState.IsValid)
            {
                db.Entry(koronaStore).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(koronaStore);
        }

        // GET: KoronaStores/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KoronaStore koronaStore = db.KoronaStores.Find(id);
            if (koronaStore == null)
            {
                return HttpNotFound();
            }
            return View(koronaStore);
        }

        // POST: KoronaStores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            KoronaStore koronaStore = db.KoronaStores.Find(id);
            db.KoronaStores.Remove(koronaStore);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
