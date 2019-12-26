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
        [Authorize(Roles=nameof(RoleEnum.CompanyManager) + ", " + nameof(RoleEnum.Employee))]
        public ActionResult Index(bool viewAll = false, int? id = null)
        {
            var user = db.Employees.Find(User.Identity.GetUserId());

            if(id != null)
            {
                var station = db.Stations.Find(id);
                id = station.CompanyId != user.CompanyId ? null : id;
            }

            List<Appointment> appointments = null;
            if (User.IsInRole(nameof(RoleEnum.CompanyManager)))
            {
                if (viewAll)
                {
                    if(id != null)
                    {
                        appointments = user.Company.Appointments
                            .Where(s => s.StationId == (int)id).ToList();
                    }
                    else
                    {
                        appointments = user.Company.Appointments.ToList();
                    }
                }
                else
                {
                    var dayLimit = DateTime.Now.AddDays(1);

                    if(id != null)
                    {
                        appointments = user.Company.Appointments
                            .Where(ap => ap.AppointmentStatusId == AppointmentStatusEnum.Pending ||
                            ap.AppointmentStatusId == AppointmentStatusEnum.Ongoing &&
                            ap.Start < dayLimit &&
                            ap.StationId == id)
                            .ToList();
                    }
                    else
                    {
                        appointments = user.Company.Appointments
                            .Where(ap => ap.AppointmentStatusId == AppointmentStatusEnum.Pending ||
                            ap.AppointmentStatusId == AppointmentStatusEnum.Ongoing &&
                            ap.Start < dayLimit)
                            .ToList();
                    }
                }
            }
            else
            {
                if(user.StationId == null)
                {
                    return HttpNotFound();
                }

                if(viewAll)
                {
                    appointments = user.Station.Appointments.ToList();
                }
                else
                {
                    var dayLimit = DateTime.Now.AddDays(1);

                    appointments = user.Station.Appointments
                            .Where(ap => ap.AppointmentStatusId == AppointmentStatusEnum.Pending ||
                            ap.AppointmentStatusId == AppointmentStatusEnum.Ongoing &&
                            ap.Start < dayLimit)
                            .ToList();
                }

            }

            appointments.OrderByDescending(a => a.Start);

            return View(appointments.AsQueryable());
        }

        // GET: Appointments/Details/5
        [Authorize(Roles = nameof(RoleEnum.CompanyManager) + ", " + nameof(RoleEnum.Employee))]
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
        //[Authorize(Roles = nameof(RoleEnum.Employee))]
        public ActionResult Create()
        {
            ViewBag.PodId = new SelectList(db.Pods, "Id", "Id");
            ViewBag.StationId = new SelectList(db.Stations, "Id", "ComercialName");
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatuses, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");

            var user = db.Employees.Find(User.Identity.GetUserId());
            int companyId = user.CompanyId;

            return View(new Appointment() { CompanyId = companyId, Company = user.Company, Start = DateTime.Now.AddMinutes(30), End = DateTime.Now.AddHours(5) });
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = nameof(RoleEnum.Employee))]
        public ActionResult Create([Bind(Include = "Id,CompanyId,StationId,PodId,UserId,Cost,Start,End,AppointmentStatusId")] Appointment appointment)
        {
            var user = db.Employees.Find(User.Identity.GetUserId());
            var prices = db.Stations.Where(s => s.Id == appointment.StationId).Select(s => s.Prices).FirstOrDefault();
            appointment.CompanyId = user.CompanyId;
            appointment.Cost = PriceGenerator.CalculatePrice(appointment.Start, appointment.End, prices, db.Pods.Find(appointment.PodId).PodType.Id);

            if (ModelState.IsValid)
            {
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
