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
    public class AppointmentsController : Controller
    {
        private ERecargaDbContext db = new ERecargaDbContext();

        // GET: Appointments
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.Company).Include(a => a.Pod).Include(a => a.Station).Include(a => a.Status).Include(a => a.User);
            return View(appointments.ToList());
        }

        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name");
            ViewBag.PodId = new SelectList(db.Pods, "Id", "Id");
            ViewBag.StationId = new SelectList(db.Stations, "Id", "ComercialName");
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatuses, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyId,StationId,PodId,UserId,Cost,Start,End,AppointmentStatusId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", appointment.CompanyId);
            ViewBag.PodId = new SelectList(db.Pods, "Id", "Id", appointment.PodId);
            ViewBag.StationId = new SelectList(db.Stations, "Id", "ComercialName", appointment.StationId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatuses, "Id", "Name", appointment.AppointmentStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", appointment.UserId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", appointment.CompanyId);
            ViewBag.PodId = new SelectList(db.Pods, "Id", "Id", appointment.PodId);
            ViewBag.StationId = new SelectList(db.Stations, "Id", "ComercialName", appointment.StationId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatuses, "Id", "Name", appointment.AppointmentStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", appointment.UserId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,StationId,PodId,UserId,Cost,Start,End,AppointmentStatusId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", appointment.CompanyId);
            ViewBag.PodId = new SelectList(db.Pods, "Id", "Id", appointment.PodId);
            ViewBag.StationId = new SelectList(db.Stations, "Id", "ComercialName", appointment.StationId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatuses, "Id", "Name", appointment.AppointmentStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", appointment.UserId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
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
