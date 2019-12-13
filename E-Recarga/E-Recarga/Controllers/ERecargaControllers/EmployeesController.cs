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
    public class EmployeesController : Controller
    {
        private ERecargaDbContext db = new ERecargaDbContext();

        // GET: Employees
        public ActionResult Index()
        {
            var users = db.Employees.Include(e => e.Company).Include(e => e.Station);
            return View(users.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [NonAction]
        private void CreateEmployeeHelper(int companyId, int? stationId)
        {
            if (User.Identity.AuthenticationType == nameof(RoleEnum.Administrator))
            {
                ViewBag.Role = nameof(RoleEnum.CompanyManager);
                ViewBag.StationId = null;
            }
            else
            {
                var loggedUserID = User.Identity.GetUserId();
                var manager = db.Employees.Where(user => user.Id == loggedUserID).SingleOrDefault();

                if (stationId == null)
                {
                    ViewBag.StationId = new SelectList(db.Stations.Where(s => s.CompanyId == manager.CompanyId), "Id", "ComercialName");
                }
                else
                {
                    ViewBag.StationId = stationId;
                    ViewBag.Role = nameof(RoleEnum.Employee);
                }
            }

            ViewBag.CompanyId = companyId;
        }
        // GET: Employees/Create
        public ActionResult Create(int companyId, int? stationId)
        {
            CreateEmployeeHelper(companyId, stationId);
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int companyId, int? stationId,[Bind(Include = "Id,Name,Wallet,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,StationId,CompanyId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            CreateEmployeeHelper(companyId, stationId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", employee.CompanyId);
            ViewBag.StationId = new SelectList(db.Stations, "Id", "ComercialName", employee.StationId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Wallet,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,StationId,CompanyId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", employee.CompanyId);
            ViewBag.StationId = new SelectList(db.Stations, "Id", "ComercialName", employee.StationId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Employee employee = db.Employees.Find(id);
            db.Users.Remove(employee);
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
