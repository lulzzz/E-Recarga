using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Recarga.Models;
using E_Recarga.Models.ERecargaModels;
using Microsoft.AspNet.Identity;

namespace E_Recarga.Controllers.ERecargaControllers
{
    public class StationsController : Controller
    {
        private ERecargaDbContext db = new ERecargaDbContext();

        // GET: Stations
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult Index()
        {
            var company = db.Employees.Find(User.Identity.GetUserId()).Company;
            var stations = company.Stations;
            return View(stations.ToList());
        }

        // GET: Stations/Details/5
        [Authorize(Roles = nameof(RoleEnum.CompanyManager) + "," + nameof(RoleEnum.Employee))]
        public ActionResult Details(int? id)
        {
            Station station;

            if (User.IsInRole(nameof(RoleEnum.CompanyManager)))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                station = db.Stations.Find(id);
                if (station == null)
                {
                    return HttpNotFound();
                }
            }
            else
            {
                var user = db.Employees.Find(User.Identity.GetUserId());
                if(user.Station == null)
                    return HttpNotFound();

                station = user.Station;
            }

            return View(station);
        }

        // GET: Stations/Create
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult Create([Bind(Include = "ComercialName,StreetName,BuildingNumber,PostalCode,Parish,Region")] Station station)
        {
            var user = db.Employees.Find(User.Identity.GetUserId());
            station.CompanyId = user.CompanyId;

            if (ModelState.IsValid)
            {
                db.Stations.Add(station);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(station);
        }

        // GET: Stations/Edit/5
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult Edit(int? id)
        {
            var user = db.Employees.Find(User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            else if(station.CompanyId != user.CompanyId)
            {
                return HttpNotFound();
            }

            return View(station);
        }

        // POST: Stations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult Edit([Bind(Include = "ComercialName,StreetName,BuildingNumber,PostalCode,Parish,Region")] Station station)
        {
            if (ModelState.IsValid)
            {
                db.Entry(station).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(station);
        }
        //TODO: Attempt to change the ID from the model from the client
        // GET: Stations/Delete/5
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult Delete(int? id)
        {
            var user = db.Employees.Find(User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            else if (station.CompanyId != user.CompanyId)
            {
                return HttpNotFound();
            }

            return View(station);
        }

        // POST: Stations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult DeleteConfirmed(int id)
        {
            Station station = db.Stations.Find(id);
            db.Stations.Remove(station);
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
