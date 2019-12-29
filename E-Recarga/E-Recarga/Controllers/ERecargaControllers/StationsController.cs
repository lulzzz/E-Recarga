﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Recarga.App_Code;
using E_Recarga.Models;
using E_Recarga.Models.ERecargaModels;
using E_Recarga.ViewModels;
using Microsoft.AspNet.Identity;

namespace E_Recarga.Controllers.ERecargaControllers
{
    [RoutePrefix("Estacoes")]
    public class StationsController : Controller
    {
        private ERecargaDbContext db = new ERecargaDbContext();

        // GET: Stations
        [Route]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public PartialViewResult IndexGrid(string search)
        {
            var company = db.Employees.Find(User.Identity.GetUserId()).Company;
            var stations = company.Stations;

            return PartialView("_StationIndexPartialGrid", stations.Where(x => x.StreetName.ToLower().Contains(search.ToLower()) || 
                                                                            x.Region.ToLower().Contains(search.ToLower()) ||
                                                                            x.Parish.ToLower().Contains(search.ToLower()) ||
                                                                            x.ComercialName.ToLower().Contains(search.ToLower())).AsQueryable());
        }

        // GET: Stations/Details/5
        [Route("Detalhes")]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager) + "," + nameof(RoleEnum.Employee))]
        public ActionResult Details(int? id)
        {
            Station station;
            var user = db.Employees.Find(User.Identity.GetUserId());

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
                if(station.CompanyId != user.CompanyId)
                {
                    return HttpNotFound();
                }

            }
            else
            {
                if(user.Station == null)
                {
                    return HttpNotFound();
                }

                station = user.Station;
            }

            return View(station);
        }

        // GET: Stations/Create
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        [Route("Criar")]
        public ActionResult Create()
        {
            var company = db.Employees.Find(User.Identity.GetUserId()).Company;
            Station station = new Station();
            StationViewModel viewModel = new StationViewModel() { _Station = station, CompanyName = company.Name };

            return View(viewModel);
        }

        // POST: Stations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Criar")]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult Create(StationViewModel viewModel)
        {
            var user = db.Employees.Find(User.Identity.GetUserId());
            viewModel._Station.CompanyId = user.CompanyId;

            if (ModelState.IsValid)
            {
                db.Stations.Add(viewModel._Station);

                foreach (var price in ScheduleGenerator.GeneratePrices(Double.Parse(viewModel.NormalCost), Double.Parse(viewModel.FastCost))){
                    price.StationId = viewModel._Station.Id;
                    db.Prices.Add(price);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: Stations/Edit/5
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        [Route("{id:int}/Editar")]
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
        [Route("{id:int}/Editar")]
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
        [Route("{id:int}/Apagar")]
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
        [Route("{id:int}/Apagar")]
        [Authorize(Roles = nameof(RoleEnum.CompanyManager))]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Employees.Find(User.Identity.GetUserId());
            Station station = db.Stations.Find(id);
            var employees = station.Employees;
            var pods = station.Pods;
            var appointments = station.Appointments;

            if (user.CompanyId != station.CompanyId)
            {
                return HttpNotFound();
            }

            appointments.ToList()
                .ForEach(r =>
                {
                    r.StationId = null;
                    db.Entry(r).State = EntityState.Modified;
                }
            );

            pods.ToList()
                .ForEach(r =>
                {
                    r.Appointments.ToList().ForEach(a =>
                    {
                        a.PodId = null;
                    });

                    db.Entry(r).State = EntityState.Deleted;
                }
            );

            employees.ToList()
                .ForEach(r =>
                    {
                        r.StationId = null;
                        db.Entry(r).State = EntityState.Modified;
                    }
                );

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
