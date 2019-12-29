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
    [RoutePrefix("Postos")]
    public class PodsController : Controller
    {
        private ERecargaDbContext db = new ERecargaDbContext();

        // GET: Pods
        [Route]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager) + "," + nameof(RoleEnum.Employee))]
        public ActionResult Index()
        {
            var user = db.Employees.Find(User.Identity.GetUserId());
            IQueryable<Pod> pods;

            if (User.IsInRole(nameof(RoleEnum.CompanyManager)))
            {
                pods = db.Pods
                        .Include(p => p.PodType)
                        .Include(p => p.Station)
                        .Where(p => p.Station.CompanyId == user.CompanyId);
            }
            else
            {
                if(user.StationId != null)
                {
                    pods = db.Pods
                            .Include(p => p.PodType)
                            .Include(p => p.Station)
                            .Where(p => p.StationId == user.StationId);
                }
                else
                {
                    pods = new List<Pod>().AsQueryable();
                }
            }

            return View(pods.ToList());
        }

        // GET: Pods/Details/5
        [Route("{id:int}/Detalhes")]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager) + "," + nameof(RoleEnum.Employee))]
        public ActionResult Details(int? id)
        {
            var user = db.Employees.Find(User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pod pod = db.Pods.Find(id);
            if (pod == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole(nameof(RoleEnum.CompanyManager)))
            {
                if(pod.Station.CompanyId != user.CompanyId)
                    return HttpNotFound();
            }
            else
            {
                if (pod.Station.Id != user.StationId)
                    return HttpNotFound();
            }

            return View(pod);
        }

        // GET: Pods/Create
        [Route("Criar")]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult Create()
        {
            var company = db.Employees.Find(User.Identity.GetUserId()).Company;

            ViewBag.PodId = new SelectList(db.PodTypes, "Id", "Name");
            ViewBag.StationId = new SelectList(company.Stations, "Id", "ComercialName");
            return View();
        }

        // POST: Pods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Criar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult Create([Bind(Include = "StationId,PodId,isActive")] Pod pod)
        {
            var company = db.Employees.Find(User.Identity.GetUserId()).Company;
            var stations = company.Stations.Where(s => s.Id == pod.StationId).SingleOrDefault();
            pod.Identifier = stations.Pods.Count > 0? stations.Pods.Select(p => p.Identifier).Max() + 1 : 1;

            if (ModelState.IsValid)
            {
                db.Pods.Add(pod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PodId = new SelectList(db.PodTypes, "Id", "Name", pod.PodId);
            ViewBag.StationId = new SelectList(company.Stations, "Id", "ComercialName", pod.StationId);
            return View(pod);
        }

        // GET: Pods/Edit/5
        [Route("{id:int}/Editar")]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager) + "," + nameof(RoleEnum.Employee))]
        public ActionResult Edit(int? id)
        {
            var user = db.Employees.Find(User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pod pod = db.Pods.Find(id);
            if (pod == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole(nameof(RoleEnum.CompanyManager)))
            {
                if (pod.Station.CompanyId != user.CompanyId)
                    return HttpNotFound();
            }
            else
            {
                if (pod.Station.Id != user.StationId)
                    return HttpNotFound();
            }

            ViewBag.PodId = new SelectList(db.PodTypes, "Id", "Name", pod.PodId);
            ViewBag.StationId = new SelectList(user.Company.Stations, "Id", "ComercialName", pod.StationId);

            return View(pod);
        }

        // POST: Pods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("{id:int}/Editar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager) + "," + nameof(RoleEnum.Employee))]
        public ActionResult Edit([Bind(Include = "PodId, StationId,Id")]Pod pod)
        {
            var company = db.Employees.Find(User.Identity.GetUserId()).Company;
            Pod temp = db.Pods.Find(pod.Id);
            temp.PodId = pod.PodId;

            if (temp.Station.Id != pod.StationId)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PodId = new SelectList(db.PodTypes, "Id", "Name", pod.PodId);
            ViewBag.StationId = new SelectList(company.Stations, "Id", "ComercialName", pod.StationId);
            return View(pod);
        }

        // GET: Pods/Delete/5
        [Route("{id:int}/Apagar")]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult Delete(int? id)
        {
            var user = db.Employees.Find(User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pod pod = db.Pods.Find(id);
            if (pod == null)
            {
                return HttpNotFound();
            }

            if (pod.Station.CompanyId != user.CompanyId)
            {
                return HttpNotFound();
            }

            return View(pod);
        }

        // POST: Pods/Delete/5
        [Route("{id:int}/Apagar")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult DeleteConfirmed(int id)
        {
            Pod pod = db.Pods.Find(id);

            var appointments = pod.Appointments;
            appointments.ToList()
                .ForEach(r =>
                {
                    r.PodId = null;
                    db.Entry(r).State = EntityState.Modified;
                }
            );

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
