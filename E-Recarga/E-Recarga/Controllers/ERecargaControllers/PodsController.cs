using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Recarga.Models.ERecargaModels;

namespace E_Recarga.Controllers.ERecargaControllers
{
    public class PodsController : Controller
    {
        private ERecargaDbContext db = new ERecargaDbContext();

        // GET: Pods
        public ActionResult Index()
        {
            var pods = db.Pods.Include(p => p.PodType).Include(p => p.Station);
            return View(pods.ToList());
        }

        // GET: Pods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pod pod = db.Pods.Find(id);
            if (pod == null)
            {
                return HttpNotFound();
            }
            return View(pod);
        }

        // GET: Pods/Create
        public ActionResult Create()
        {
            ViewBag.PodId = new SelectList(db.PodTypes, "Id", "Name");
            ViewBag.StationId = new SelectList(db.Stations, "Id", "ComercialName");
            return View();
        }

        // POST: Pods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StationId,PodId,isActive")] Pod pod)
        {
            if (ModelState.IsValid)
            {
                db.Pods.Add(pod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PodId = new SelectList(db.PodTypes, "Id", "Name", pod.PodId);
            ViewBag.StationId = new SelectList(db.Stations, "Id", "ComercialName", pod.StationId);
            return View(pod);
        }

        // GET: Pods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pod pod = db.Pods.Find(id);
            if (pod == null)
            {
                return HttpNotFound();
            }
            ViewBag.PodId = new SelectList(db.PodTypes, "Id", "Name", pod.PodId);
            ViewBag.StationId = new SelectList(db.Stations, "Id", "ComercialName", pod.StationId);
            return View(pod);
        }

        // POST: Pods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StationId,PodId,isActive")] Pod pod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PodId = new SelectList(db.PodTypes, "Id", "Name", pod.PodId);
            ViewBag.StationId = new SelectList(db.Stations, "Id", "ComercialName", pod.StationId);
            return View(pod);
        }

        // GET: Pods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pod pod = db.Pods.Find(id);
            if (pod == null)
            {
                return HttpNotFound();
            }
            return View(pod);
        }

        // POST: Pods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pod pod = db.Pods.Find(id);
            db.Pods.Remove(pod);
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
