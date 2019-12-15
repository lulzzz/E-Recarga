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

    public class CompaniesController : Controller
    {
        private ERecargaDbContext db = new ERecargaDbContext();

        // GET: Companies
        [Authorize(Roles = nameof(RoleEnum.Administrator))]
        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }

        [Authorize(Roles = nameof(RoleEnum.Administrator))]
        public ActionResult Managers()
        {
            var id = db.Roles
                .Where(role => role.Name == nameof(RoleEnum.CompanyManager))
                .FirstOrDefault().Id;

            var employees = from u in db.Employees
                            where u.Roles.Any(r => r.RoleId == id)
                            select u;

            return RedirectToAction("Index", "Employees", employees);
        }

        // GET: Companies/Details/5
        [Authorize(Roles = nameof(RoleEnum.CompanyManager) + "," + nameof(RoleEnum.Administrator))]
        public ActionResult Details(int? id)
        {
            Company company;

            if (!User.IsInRole(nameof(RoleEnum.CompanyManager)))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                company = db.Companies.Find(id);
                if (company == null)
                {
                    return HttpNotFound();
                }
            }
            else
            {
                var userId = User.Identity.GetUserId();
                var user = db.Employees.Include(x => x.Company)
                            .Where(x => x.Id == userId).SingleOrDefault();
                company = user.Company;
            }

            return View(company);
        }

        // GET: Companies/Create
        [Authorize(Roles = nameof(RoleEnum.Administrator))]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.Administrator))]
        public ActionResult Create([Bind(Include = "Id,Name,PhoneNumber,Email")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        // GET: Companies/Edit/5
        [Authorize(Roles = nameof(RoleEnum.CompanyManager) + "," + nameof(RoleEnum.Administrator))]
        public ActionResult Edit(int? id)
        {
            Company company;

            if (!User.IsInRole(nameof(RoleEnum.CompanyManager)))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                company = db.Companies.Find(id);
                if (company == null)
                {
                    return HttpNotFound();
                }
            }
            else
            {
                var userId = User.Identity.GetUserId();
                var user = db.Employees.Include(x => x.Company)
                            .Where(x => x.Id == userId).SingleOrDefault();
                company = user.Company;
            }

            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager) + "," + nameof(RoleEnum.Administrator))]
        public ActionResult Edit([Bind(Include = "Id,Name,PhoneNumber,Email")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();

                if (User.IsInRole(nameof(RoleEnum.CompanyManager)))
                    return RedirectToAction("Details");

                return RedirectToAction("Index");
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        [Authorize(Roles = nameof(RoleEnum.Administrator))]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.Administrator))]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
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
