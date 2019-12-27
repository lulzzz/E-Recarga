using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using E_Recarga.Models;
using E_Recarga.Models.ERecargaModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using E_Recarga.ViewModels;
using System.Collections.Generic;
using E_Recarga.ViewModels.EmployeeVMs;

namespace E_Recarga.Controllers.ERecargaControllers
{
    [RoutePrefix("Empregados")]
    public class EmployeesController : Controller
    {
        private ERecargaDbContext db = new ERecargaDbContext();

        // GET: Employees
        [Route]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult Index()
        {
            var company = db.Employees.Find(User.Identity.GetUserId()).Company;
            List<Employee> employees = company.Employees.ToList();

            EmployeeIndexViewModel employeesIndex = new EmployeeIndexViewModel();
            List<EmployeeViewModel> employeesList = new List<EmployeeViewModel>();
            foreach (Employee elem in employees)
            {
                var roleId = elem.Roles.FirstOrDefault().RoleId;
                var role = db.Roles.Where(x => x.Id == roleId).FirstOrDefault();
                employeesList.Add(new EmployeeViewModel() { Employee = elem, EmployeeType = role.Name });
            }
            employeesIndex.EmployeesVM = employeesList.AsQueryable();

            return View(employeesIndex);
        }

        // GET: Employees/Details/5
        [Route("{id}/Detalhes")]
        [Authorize(Roles = nameof(RoleEnum.Administrator) + "," + nameof(RoleEnum.CompanyManager))]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id.Trim());
            if (employee == null)
            {
                return HttpNotFound();
            }

            var check = employee.Roles.FirstOrDefault().RoleId;
            var role = db.Roles.Where(x => x.Id == check).FirstOrDefault();

            if (User.IsInRole(nameof(RoleEnum.Administrator)))
            {
                var managerRole = db.Roles.Where(r => r.Name == nameof(RoleEnum.CompanyManager)).Select(x => x.Id).Single();

                if (role.Id != managerRole)
                {
                    return HttpNotFound();
                }
            }

            EmployeeViewModel employeeVM = new EmployeeViewModel(employee);
            Enum_Dictionnary.Translator.TryGetValue(role.Name, out string pt_role);
            employeeVM.EmployeeType = pt_role;

            return View(employeeVM);
        }

        [NonAction]
        private void CreateEmployeeHelper(EmployeeViewModel employeeVM)
        {
            string pt_role_manager, pt_role_employee;
            Enum_Dictionnary.Translator.TryGetValue(nameof(RoleEnum.CompanyManager), out pt_role_manager);
            Enum_Dictionnary.Translator.TryGetValue(nameof(RoleEnum.Employee), out pt_role_employee);

            employeeVM.RoleEnums = new List<string>();

            if (User.IsInRole(nameof(RoleEnum.Administrator)))
            {
                if(employeeVM.CompanyId == 0)
                    employeeVM.CompaniesList = new SelectList(db.Companies, "Id", "Name");
                else
                    employeeVM.CompaniesList = new SelectList(db.Companies.Where(c=>c.Id==employeeVM.CompanyId), "Id", "Name");

                employeeVM.RoleEnums.Add(pt_role_manager);
            }
            else
            {
                employeeVM.CompaniesList = new SelectList(db.Companies.Where(c => c.Id == employeeVM.CompanyId), "Id", "Name");

                if (employeeVM.StationId == null)
                {
                    employeeVM.RoleEnums.Add(pt_role_employee);
                    employeeVM.RoleEnums.Add(pt_role_manager);
                }
                else
                {
                    employeeVM.RoleEnums.Add(pt_role_employee);
                }
            }
        }

        [Route("Criar")]
        [Authorize(Roles = nameof(RoleEnum.Administrator) + "," + nameof(RoleEnum.CompanyManager))]
        // GET: Employees/Create
        public ActionResult Create(int? companyId, int? stationId)
        {
            if (stationId != null && User.IsInRole(nameof(RoleEnum.Administrator)))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            int company_Id = 0;

            if (User.IsInRole(nameof(RoleEnum.CompanyManager)))
            {
                var loggedUserID = User.Identity.GetUserId();
                var manager = db.Employees.Where(user => user.Id == loggedUserID).SingleOrDefault();
                company_Id = manager.CompanyId;

                if (stationId != null)
                {
                    Station station = db.Stations.Find(stationId);
                    Company company = db.Companies.Find(company_Id);

                    if (station == null || company.Stations.Contains(station) == false)
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                if (companyId != null)
                    company_Id = (int) companyId;
            }

            EmployeeViewModel employeeVM = new EmployeeViewModel() {StationId = stationId, CompanyId = company_Id};
            CreateEmployeeHelper(employeeVM);
            return View(employeeVM);
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Criar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.Administrator) + "," + nameof(RoleEnum.CompanyManager))]
        public ActionResult Create(EmployeeViewModel employeeVM) //TODO: BINDS EXCLUDE PARAMeters
        {
            Employee employee = new Employee();
            if (ModelState.IsValid)
            {
                employee.Name = employeeVM.Name;
                employee.UserName = employee.Email = employeeVM.Email;
                employee.PhoneNumber = employeeVM.PhoneNumber;
                employee.CompanyId = employeeVM.CompanyId;
                employee.StationId = employeeVM.StationId;

                string role = Enum_Dictionnary.Translator[employeeVM.EmployeeType];
           
                //Create Users
                var store = new UserStore<ApplicationUser>(new ERecargaDbContext());
                var manager = new UserManager<ApplicationUser>(store);

                string password = "" + employee.Name.Trim(' ') + employee.PhoneNumber + ".";

                var res = manager.Create(employee, password);
                if (res.Succeeded)
                    manager.AddToRole(employee.Id, role);
                //

                return RedirectToAction("Index");
            }

            CreateEmployeeHelper(employeeVM);
            return View(employeeVM);
        }

        [NonAction]
        private void EditHelper(EmployeeViewModel employeeVM)
        {
            string pt_role_manager, pt_role_employee;
            Enum_Dictionnary.Translator.TryGetValue(nameof(RoleEnum.CompanyManager), out pt_role_manager);
            Enum_Dictionnary.Translator.TryGetValue(nameof(RoleEnum.Employee), out pt_role_employee);

            employeeVM.RoleEnums = new List<string> {
                pt_role_manager,
                pt_role_employee
            };

            employeeVM.StationsList = new SelectList(db.Stations.Where(s => s.CompanyId == employeeVM.CompanyId), "Id", "ComercialName", (employeeVM.StationId).ToString());
            employeeVM.CompaniesList = new SelectList(db.Companies,"Id","Name",(employeeVM.CompanyId).ToString());
        }

        // GET: Employees/Edit/5
        [Route("{id}/Editar")]
        [Authorize(Roles = nameof(RoleEnum.Administrator) + "," + nameof(RoleEnum.CompanyManager))]
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

            var loggedUserID = User.Identity.GetUserId();
            var manager = db.Employees.Where(user => user.Id == loggedUserID).SingleOrDefault();

            if (User.IsInRole(nameof(RoleEnum.CompanyManager)) && employee.CompanyId != manager.CompanyId)
            {
                return HttpNotFound();
            }

            var roleId = employee.Roles.FirstOrDefault().RoleId;
            string roleName = db.Roles.Where(x => x.Id == roleId).FirstOrDefault().Name;

            if (User.IsInRole(nameof(RoleEnum.Administrator)) && roleName != nameof(RoleEnum.CompanyManager))
            {
                return HttpNotFound();
            }

            string pt_role;
            Enum_Dictionnary.Translator.TryGetValue(roleName, out pt_role);

            EmployeeViewModel employeeVM = new EmployeeViewModel() {Name = employee.Name,
                Username = employee.UserName, Email = employee.Email, PhoneNumber= employee.PhoneNumber,
                EmployeeType = pt_role, CompanyName = employee.Company.Name, CompanyId = employee.CompanyId,
                StationId = employee.StationId, Id = id};

            EditHelper(employeeVM);
            return View(employeeVM);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("{id}/Editar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.Administrator) + "," + nameof(RoleEnum.CompanyManager))]
        public ActionResult Edit(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                Employee employee = db.Employees.Find(employeeVM.Id);
                employee.Name = employeeVM.Name;
                employee.PhoneNumber = employeeVM.PhoneNumber;
                employee.UserName = employee.Email = employeeVM.Email;
                
                if (User.IsInRole(nameof(RoleEnum.Administrator)))
                    employee.CompanyId = employeeVM.CompanyId;
                else
                {
                    string employee_role = Enum_Dictionnary.Translator[employeeVM.EmployeeType];

                    if (employee_role == nameof(RoleEnum.CompanyManager))
                    {
                        employee.StationId = null;
                    }
                    else
                    {
                        employee.StationId = employeeVM.StationId;
                    }

                    //Mudar os roles
                    var roleId = employee.Roles.FirstOrDefault().RoleId;
                    var role = db.Roles.Where(x => x.Id == roleId).FirstOrDefault();

                    if (role.Name != employee_role)
                    {
                        var store = new UserStore<ApplicationUser>(new ERecargaDbContext());
                        var manager = new UserManager<ApplicationUser>(store);
                        manager.RemoveFromRole(employee.Id, role.Name);
                        manager.AddToRole(employee.Id, employee_role);
                    }
                    //
                }
                
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            EditHelper(employeeVM);
            return View(employeeVM);
        }

        // GET: Employees/Delete/5
        [Route("{id}/Apagar")]
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
        [Route("{id}/Apagar")]
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
