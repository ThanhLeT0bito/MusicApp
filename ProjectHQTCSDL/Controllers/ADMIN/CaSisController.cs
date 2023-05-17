using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectHQTCSDL.Models;

namespace ProjectHQTCSDL.Controllers.ADMIN
{
    public class CaSisController : Controller
    {
        private ProjectMusicEntities db = new ProjectMusicEntities();

        // GET: CaSis
        public ActionResult Index()
        {
            return View(db.CaSis.ToList());
        }

        // GET: CaSis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaSi caSi = db.CaSis.Find(id);
            if (caSi == null)
            {
                return HttpNotFound();
            }
            return View(caSi);
        }

        // GET: CaSis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CaSis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCaSi,tenCaSi,imgCaSi")] CaSi caSi)
        {
            if (ModelState.IsValid)
            {
                db.CaSis.Add(caSi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(caSi);
        }

        // GET: CaSis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaSi caSi = db.CaSis.Find(id);
            if (caSi == null)
            {
                return HttpNotFound();
            }
            return View(caSi);
        }

        // POST: CaSis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCaSi,tenCaSi,imgCaSi")] CaSi caSi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(caSi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(caSi);
        }

        // GET: CaSis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaSi caSi = db.CaSis.Find(id);
            if (caSi == null)
            {
                return HttpNotFound();
            }
            return View(caSi);
        }

        // POST: CaSis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CaSi caSi = db.CaSis.Find(id);
            db.CaSis.Remove(caSi);
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
