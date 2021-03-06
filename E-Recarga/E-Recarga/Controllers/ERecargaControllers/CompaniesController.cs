using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Recarga.App_Code;
using E_Recarga.Models;
using E_Recarga.Models.ERecargaModels;
using E_Recarga.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace E_Recarga.Controllers.ERecargaControllers
{
    [RoutePrefix("Empresas")]
    public class CompaniesController : Controller
    {
        private ERecargaDbContext db = new ERecargaDbContext();

        // GET: Companies
        [Route]
        [Authorize(Roles = nameof(RoleEnum.Administrator))]
        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }

        [Route("Gestores")]
        [Authorize(Roles = nameof(RoleEnum.Administrator))]
        public ActionResult Managers()
        {
            var id = db.Roles
                .Where(role => role.Name == nameof(RoleEnum.CompanyManager))
                .FirstOrDefault().Id;

            var employees = from u in db.Employees
                            where u.Roles.Any(r => r.RoleId == id)
                            select u;

            return View(employees.ToList());
        }

        [Route("Detalhes")]
        // GET: Companies/Details/5
        [Authorize(Roles = nameof(RoleEnum.CompanyManager) + "," + nameof(RoleEnum.Administrator))]
        public ActionResult Details(int? id)
        {
            CompanyViewModel viewModel = new CompanyViewModel();

            if (!User.IsInRole(nameof(RoleEnum.CompanyManager)))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                viewModel.Company = db.Companies.Find(id);
                if (viewModel.Company == null)
                {
                    return HttpNotFound();
                }

                var role = db.Roles
                .Where(x => x.Name == nameof(RoleEnum.CompanyManager))
                .FirstOrDefault().Id;
                
                viewModel.Managers = viewModel.Company
                    .Employees.Where(x => x.Roles.Any(r => r.RoleId == role)).ToList();
            }
            else
            {
                var userId = User.Identity.GetUserId();
                var user = db.Employees.Include(u => u.Company).Include(u=>u.Roles).ToList()
                            .Where(x => x.Id == userId).SingleOrDefault();
                viewModel.Company = user.Company;

                Dictionary<Employee,string> employees = new Dictionary<Employee, string>();
                var companyEmployees = viewModel.Company.Employees.ToList();
                var roles = db.Roles.ToList();

                companyEmployees.ForEach(e =>
                {
                    var myRoleId = e.Roles.FirstOrDefault().RoleId;
                    var myRole = roles.Where(r => r.Id == myRoleId).FirstOrDefault().Name;

                    employees.Add(e, Enum_Dictionnary.Translator[myRole]);
                });
                viewModel.EmployeeRoleDictionary = employees;
            }
            return View(viewModel);
        }

        // GET: Companies/Create
        [Route("Criar")]
        [Authorize(Roles = nameof(RoleEnum.Administrator))]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Criar")]
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
        [Route("{id:int}/Editar")]
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
        [Route("{id:int}/Editar")]
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
        [Route("{id:int}/Apagar")]
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
        [Route("{id:int}/Apagar")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.Administrator))]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);

            db.Appointments.RemoveRange(company.Appointments.ToList());
            db.Employees.RemoveRange(company.Employees.ToList());
            db.Stations.RemoveRange(company.Stations.ToList());

            db.Companies.Remove(company);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Route("CetroDeControlo")]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult DashBoard()
        {
            DashboardViewModel model;
            var companyId = db.Employees.Find(User.Identity.GetUserId()).CompanyId;

            var stations = db.Stations.Include(x=>x.Appointments)
                .Include(x=>x.Prices).Include(x=>x.Pods).Where(x=>x.CompanyId == companyId).ToList();

            model = DataHandler.GetDashboardData(stations);

            return View(model);
        }

        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        [ChildActionOnly]
        [ActionName("GetTopStation")]
        public PartialViewResult DashBoardStation(TopStation station)
        {
            return PartialView("_StationStatsDashboard", station);
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
