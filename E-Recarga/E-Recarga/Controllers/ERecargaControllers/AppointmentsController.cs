using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using E_Recarga.Models;
using E_Recarga.Models.ERecargaModels;
using Microsoft.AspNet.Identity;
using E_Recarga.App_Code;
using System.Globalization;

namespace E_Recarga.Controllers.ERecargaControllers
{
    [RoutePrefix("Marcacoes")]
    public class AppointmentsController : Controller
    {
        private ERecargaDbContext db = new ERecargaDbContext();

        // GET: Appointments
        [Authorize(Roles = nameof(RoleEnum.CompanyManager) + ", " + nameof(RoleEnum.Employee) + ", " + nameof(RoleEnum.User))]
        [Route]
        public ActionResult Index(bool viewAll = false, int? id = null)
        {
            var user = db.Employees.Find(User.Identity.GetUserId());

            if (id != null)
            {
                var station = db.Stations.Find(id);
                id = station.CompanyId != user.CompanyId ? null : id;
            }

            List<Appointment> appointments = null;
            if (User.IsInRole(nameof(RoleEnum.CompanyManager)))
            {
                if (viewAll)
                {
                    if (id != null)
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

                    if (id != null)
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
                if (user.StationId == null)
                {
                    return HttpNotFound();
                }

                if (viewAll)
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
        [Route("Detalhes/{id:int}")]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager) + ", " + nameof(RoleEnum.Employee) + ", " + nameof(RoleEnum.User))]
        public ActionResult Details(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole(nameof(RoleEnum.CompanyManager)))
            {
                var user = db.Employees.Find(User.Identity.GetUserId());
                if (user.CompanyId != appointment.CompanyId)
                {
                    return HttpNotFound();
                }
            }
            else if (User.IsInRole(nameof(RoleEnum.Employee)))
            {
                var user = db.Employees.Find(User.Identity.GetUserId());
                if (user.StationId == null)
                {
                    return HttpNotFound();
                }
            }

            return View(appointment);
        }

        [Route("Utilizador/Agendar")]
        [Authorize(Roles = nameof(RoleEnum.User))]
        public ActionResult UserAppointmentCreate(string initCharge,string endCharge,string podTypeStr,int stationId)
        {
            var station = db.Stations.Find(stationId);
            var pods = station.Pods.Where(p => p.PodType.Name == podTypeStr && p.isActive);
            var podType = podTypeStr == nameof(PodTypeEnum.Fast) ? PodTypeEnum.Fast : PodTypeEnum.Normal;
            DateTime init;
            DateTime end;

            if (DateTime.TryParseExact(initCharge, "MM/dd/yyyy HH:mm:ss", new CultureInfo("pt-PT", false), DateTimeStyles.None, out init) == false ||
                DateTime.TryParseExact(endCharge, "MM/dd/yyyy HH:mm:ss", new CultureInfo("pt-PT", false), DateTimeStyles.None, out end) == false)
                return View();
                       
            double cost = PriceGenerator.CalculatePrice(init, end, station.Prices, podType);
            var user = db.Users.Find(User.Identity.GetUserId());

            if (cost> user.Wallet)
            {
                return RedirectToAction("AddMoney", "Users", null);
            }

            Pod pod = pods.ToList().Find(p=> (p.Appointments
                            .All(a => a.Start > end.AddMinutes(5) ||
                            a.End.AddMinutes(5) < init) ) ||
                            p.Appointments.Count == 0);

            return View(new Appointment() { Start = init, End = end,Pod = pod ,PodId = pod.Id, Cost = cost,Station = station,Company= station.Company, User = user});
        }

        [Route("Utilizador/Agendar")]
        [Authorize(Roles = nameof(RoleEnum.User))]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UserAppointmentCreate([Bind(Include = "Start,End,PodId,Cost")]Appointment appointment)
        {
            appointment.AppointmentStatusId = AppointmentStatusEnum.Pending;
            var user = db.Users.Find(User.Identity.GetUserId());
            var pod = db.Pods.Find(appointment.PodId);

            if(pod == null)
            {
                return HttpNotFound();
            }

            appointment.UserId = user.Id;
            appointment.PodId = pod.Id;
            appointment.StationId = pod.StationId;
            appointment.CompanyId = pod.Station.CompanyId;


            if (ModelState.IsValid)
            {
                user.Wallet -= appointment.Cost * 0.15;
                db.Entry(user).State = EntityState.Modified;

                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index","Home",null);
            }
            
            return View(appointment);
        }

        // GET: Appointments/Create
        [Route("Empregado/Agendar")]
        [Authorize(Roles = nameof(RoleEnum.Employee) +  ", "+ nameof(RoleEnum.CompanyManager))]
        public ActionResult Create()
        {
            //recebe employees
            var user = db.Employees.Find(User.Identity.GetUserId());

            ViewBag.StationId = new SelectList(db.Stations.Where(s=>s.CompanyId == user.CompanyId), "Id", "ComercialName");

            List<ApplicationUser> users = new List<ApplicationUser>();
            var db_users = db.Users.ToList();
            foreach (ApplicationUser elem in db_users)
            {
                var roleId = elem.Roles.ToList()[0].RoleId;
                var role = db.Roles.Where(x => x.Id == roleId).FirstOrDefault();

                if(role.Name == nameof(RoleEnum.User))
                    users.Add(elem);
            }
            ViewBag.UserId = new SelectList(users, "Id", "Email"); //TODO: only put users of primary user role

            int companyId = user.CompanyId;
            if(User.IsInRole(nameof(RoleEnum.Employee)) && user.StationId != null)
                return View(new Appointment() { CompanyId = companyId, Company = user.Company,
                    Start = DateTime.Now.AddMinutes(30), End = DateTime.Now.AddHours(5),
                    Station = user.Station, StationId = user.StationId });
            else
                return View(new Appointment()
                {
                    CompanyId = companyId,
                    Company = user.Company,
                    Start = DateTime.Now.AddMinutes(30),
                    End = DateTime.Now.AddHours(5)
                });
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Empregado/Agendar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.Employee) + ", " + nameof(RoleEnum.CompanyManager))]
        public ActionResult Create([Bind(Include = "Id,CompanyId,StationId,PodId,UserId,Cost,Start,End,AppointmentStatusId")] Appointment appointment)
        {
            var employee = db.Employees.Find(User.Identity.GetUserId());
            var user = db.Users.Find(appointment.UserId);
            
            var prices = db.Stations.Where(s => s.Id == appointment.StationId).Select(s => s.Prices).FirstOrDefault();
            if (prices.Count == 0)
            {
                return new HttpStatusCodeResult(404, "Error:" + "The station prices are not estabilished. Appointment will not be created.");
            }

            List<Pod> pods = null;

            if (User.IsInRole(nameof(RoleEnum.Employee)))
            {
                pods = employee.Station.Pods.ToList();
            }
            else
            {
                pods = employee.Company.Stations.Where(x => x.Id == appointment.StationId)
                        .SingleOrDefault().Pods.ToList();
            }

            appointment.PodId = pods.Find(p => p.Appointments
                        .All(a => a.Start > appointment.End.AddMinutes(5) ||
                        a.End.AddMinutes(5) < appointment.Start) || p.Appointments.Count == 0).Id;

            appointment.AppointmentStatusId = AppointmentStatusEnum.Pending;

            appointment.CompanyId = employee.CompanyId;
            appointment.Cost = PriceGenerator.CalculatePrice(appointment.Start, appointment.End, prices, db.Pods.Find(appointment.PodId).PodType.Id);

            if (appointment.Cost > user.Wallet)
            {
                return new HttpStatusCodeResult(404, "Error:" + "The user don''t have enough money to make the appointment");
            }
            
            if (ModelState.IsValid)
            {
                user.Wallet -= appointment.Cost * 0.15;
                db.Entry(user).State = EntityState.Modified;

                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StationId = new SelectList(db.Stations.Where(s => s.CompanyId == employee.CompanyId), "Id", "ComercialName");

            List<ApplicationUser> users = new List<ApplicationUser>();
            var db_users = db.Users.ToList();
            foreach (ApplicationUser elem in db_users)
            {
                var roleId = elem.Roles.ToList()[0].RoleId;
                var role = db.Roles.Where(x => x.Id == roleId).FirstOrDefault();

                if (role.Name == nameof(RoleEnum.User))
                    users.Add(elem);
            }
            ViewBag.UserId = new SelectList(users, "Id", "Email"); //TODO: only put users of primary user role
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        [Route("Editar/{id:int}")]
        [Authorize(Roles = nameof(RoleEnum.Employee))]
        public ActionResult Edit(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            string userId = User.Identity.GetUserId();
            int stationId = (int)db.Employees.Where(e => e.Id == userId).Select(e => e.StationId).First();

            ViewBag.PodId = new SelectList(db.Pods.Where(p => p.StationId == stationId && p.PodType == appointment.Pod.PodType), "Id", "Id", appointment.PodId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatuses, "Id", "Name", appointment.AppointmentStatusId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Editar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.Employee))]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,StationId,PodId,UserId,Cost,Start,End,AppointmentStatusId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                if (appointment.AppointmentStatusId == AppointmentStatusEnum.Completed)
                {
                    var user = db.Employees.Find(appointment.UserId);
                    user.Wallet -= appointment.Cost * 0.85;
                    db.Entry(user).State = EntityState.Modified;
                }

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
        [Route("Apagar/{id:int}")]
        [Authorize(Roles = nameof(RoleEnum.Employee) + "," + nameof(RoleEnum.CompanyManager) + "," + nameof(RoleEnum.User))]
        public ActionResult Delete(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole(nameof(RoleEnum.Employee)))
            {
                var user = db.Employees.Find(User.Identity.GetUserId());
                if (user.StationId == null)
                {
                    return HttpNotFound();
                }

                if (appointment.StationId != user.StationId)
                {
                    return HttpNotFound();
                }
            }
            else if (User.IsInRole(nameof(RoleEnum.CompanyManager)))
            {
                var user = db.Employees.Find(User.Identity.GetUserId());
                if (user.CompanyId != appointment.CompanyId)
                {
                    return HttpNotFound();
                }
            }
            else
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                if (user.Id != appointment.UserId)
                {
                    return HttpNotFound();
                }
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [Route("Apagar/{id:int}")]
        [Authorize(Roles = nameof(RoleEnum.Employee) + "," + nameof(RoleEnum.CompanyManager))]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            
            if (User.IsInRole(nameof(RoleEnum.User)))
            {
                var user = db.Users.Find(appointment.UserId);
                if (appointment.Start < DateTime.Now.AddMinutes(-30))
                {
                    user.Wallet += appointment.Cost * 0.15;
                    db.Entry(user).State = EntityState.Modified;
                }
                else
                {
                    return RedirectToAction("AppointmentsRecords", "Users", null);
                }
            }
            else
            {
                var user = db.Employees.Find(appointment.UserId);

                if (!(appointment.End < DateTime.Now))
                {
                    user.Wallet += appointment.Cost * 0.15;
                    db.Entry(user).State = EntityState.Modified;
                }
            }
                
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
