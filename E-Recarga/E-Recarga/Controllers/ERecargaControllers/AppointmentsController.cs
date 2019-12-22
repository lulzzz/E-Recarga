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
using E_Recarga.App_Code;

namespace E_Recarga.Controllers.ERecargaControllers
{
    public class AppointmentsController : Controller
    {
        private ERecargaDbContext db = new ERecargaDbContext();

        // GET: Appointments
        [Authorize(Roles=nameof(RoleEnum.CompanyManager)+", "+ nameof(RoleEnum.CompanyManager) + ", " + nameof(RoleEnum.Employee))]
        public ActionResult Index()
        {//TODO: translate status
            var appointments = db.Appointments.Include(a => a.Company).Include(a => a.Pod).Include(a => a.Station).Include(a => a.Status).Include(a => a.User);

            if (User.IsInRole(nameof(RoleEnum.CompanyManager)))
            {
                string userId = User.Identity.GetUserId();
                int companyId = db.Employees.Where(e => e.Id == userId).Select(e => e.CompanyId).First();
                appointments = appointments.Where(a => a.CompanyId == companyId);
            }
            else{
                string userId = User.Identity.GetUserId();
                appointments = appointments.Where(a => a.UserId == userId);
            }
                
            return View(appointments);
        }

        // GET: Appointments/Details/5
        [Authorize(Roles = nameof(RoleEnum.CompanyManager) + ", " + nameof(RoleEnum.CompanyManager) + ", " + nameof(RoleEnum.Employee))]
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

        [Authorize(Roles = nameof(RoleEnum.User))]
        public ActionResult UserAppointmentCreate(int stationId, int CompanyId, PodTypeEnum podType, string init_date,string end_date)
        {
            var station = db.Stations.Find(stationId);
            var pods = station.Pods.Where(p=> p.PodType.Id == podType && p.isActive);
            DateTime init = DateTime.Parse(init_date);
            DateTime end = DateTime.Parse(end_date);
            double cost = PriceGenerator.CalculatePrice(init, end, station.Prices, podType);
            Pod pod = new Pod();

            foreach (var elem in pods)
            {
                if(elem.Appointments.Where(a=>(a.End>init && a.End < end) || (a.Start < end && a.End > end) ) == null)
                {
                    pod = elem;
                    break;
                }
            }

            return View(new Appointment() { CompanyId = CompanyId, StationId = stationId, Start = init, End = end, PodId = pod.Id });
        }

        [Authorize(Roles = nameof(RoleEnum.User))]
        [HttpPost]
        public ActionResult UserAppointmentCreate([Bind(Include = "CompanyId,StationId,PodId,Cost,Start,End")] Appointment appointment)
        {
            appointment.AppointmentStatusId = Models.AppointmentStatusEnum.Pending;
            appointment.UserId = User.Identity.GetUserId();
            
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            
            return View(appointment);
        }

        // GET: Appointments/Create
        [Authorize(Roles = nameof(RoleEnum.Employee))]
        public ActionResult Create()
        {
            ViewBag.PodId = new SelectList(db.Pods, "Id", "Id");
            ViewBag.StationId = new SelectList(db.Stations, "Id", "ComercialName");
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatuses, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");

            string userId = User.Identity.GetUserId();
            int companyId = db.Employees.Where(e => e.Id == userId).Select(e => e.CompanyId).First();

            return View(new Appointment() {CompanyId = companyId, Company=db.Companies.Find(companyId)});
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.Employee))]
        public ActionResult Create([Bind(Include = "Id,CompanyId,StationId,PodId,UserId,Cost,Start,End,AppointmentStatusId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                int stationId = (int)db.Employees.Where(e => e.Id == userId).Select(e => e.StationId).First();

                var prices = db.Stations.Where(s => s.Id == stationId).Select(s => s.Prices).First();
                appointment.Cost = PriceGenerator.CalculatePrice(appointment.Start, appointment.End, prices, db.Pods.Find(appointment.PodId).PodType.Id);
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PodId = new SelectList(db.Pods, "Id", "Id", appointment.PodId);
            ViewBag.StationId = new SelectList(db.Stations, "Id", "ComercialName", appointment.StationId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatuses, "Id", "Name", appointment.AppointmentStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", appointment.UserId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        [Authorize(Roles = nameof(RoleEnum.Employee))]
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
            string userId = User.Identity.GetUserId();
            int stationId = (int)db.Employees.Where(e => e.Id == userId).Select(e => e.StationId).First();

            ViewBag.PodId = new SelectList(db.Pods.Where(p=>p.StationId == stationId && p.PodType == appointment.Pod.PodType), "Id", "Id", appointment.PodId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatuses, "Id", "Name", appointment.AppointmentStatusId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.Employee))]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,StationId,PodId,UserId,Cost,Start,End,AppointmentStatusId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            string userId = User.Identity.GetUserId();
            int stationId = (int)db.Employees.Where(e => e.Id == userId).Select(e => e.StationId).First();

            ViewBag.PodId = new SelectList(db.Pods.Where(p => p.StationId == stationId && p.PodType == appointment.Pod.PodType), "Id", "Id", appointment.PodId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatuses, "Id", "Name", appointment.AppointmentStatusId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        [Authorize(Roles = nameof(RoleEnum.User))]
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
        [Authorize(Roles = nameof(RoleEnum.User))]
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
