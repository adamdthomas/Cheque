using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HouseFly.Models;
using HouseFly.Context;

namespace HouseFly.Controllers
{
    public class TempModelsController : Controller
    {
      
        private TempContext db = new TempContext();

        // GET: TempModels
        public ActionResult Index()
        {
            return View(db.TempModels.ToList());
        }

        // GET: TempModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempModels tempModels = db.TempModels.Find(id);
            if (tempModels == null)
            {
                return HttpNotFound();
            }
            return View(tempModels);
        }

        // GET: TempModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TempModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TempId,Domain,Temp,Pressure,Humidity,TimeStamp")] TempModels tempModels)
        {
            if (ModelState.IsValid)
            {
                db.TempModels.Add(tempModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tempModels);
        }

        public void CreateInternal(TempModels tempModels)
        {
            if (ModelState.IsValid)
            {
                db.TempModels.Add(tempModels);
                db.SaveChanges();
            }
        }

        // GET: TempModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempModels tempModels = db.TempModels.Find(id);
            if (tempModels == null)
            {
                return HttpNotFound();
            }
            return View(tempModels);
        }

        // POST: TempModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TempId,Domain,Temp,Pressure,Humidity,TimeStamp")] TempModels tempModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tempModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tempModels);
        }

        // GET: TempModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempModels tempModels = db.TempModels.Find(id);
            if (tempModels == null)
            {
                return HttpNotFound();
            }
            return View(tempModels);
        }

        // POST: TempModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempModels tempModels = db.TempModels.Find(id);
            db.TempModels.Remove(tempModels);
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
