using E_Recarga.Models.ERecargaModels;
using E_Recarga.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace E_Recarga.Controllers.ERecargaControllers
{
    //[Authorize(Roles = nameof(RoleEnum.User))]
    public class UsersController : Controller
    {
        private ERecargaDbContext db = new ERecargaDbContext();

        // GET: Users
        public ActionResult Index([Bind(Include = "Region,PodType,InitCharge,EndCharge")] UserViewModel userVM)
        {
            var stations = db.Stations;
            ViewBag.Regions = stations.Select(s => s.Region).Distinct().ToList();
            ViewBag.PodTypes = db.Pods.Select(p=>(p.PodType).Name).Distinct().ToList();

            return View(userVM);
        }

        public PartialViewResult IndexGrid(string region, string init_time, string final_time, string recharge_type)
        {
            List<Station> stations = db.Stations.ToList();
            List < Station > stationsWithPodType = stations;
            DateTime initDate, finalDate;
            DateTime.TryParse(final_time, out finalDate);
            DateTime.TryParse(init_time, out initDate);

            if (!String.IsNullOrEmpty(region) )
                stations = stations.Where(s => s.Region.ToLower().Contains(region.ToLower())).ToList();

            if(init_time != null && final_time != null && initDate > DateTime.Now && finalDate > initDate)
            {
                stations = stations.FindAll(s => (s.Appointments.Any(a => a.Start > finalDate || a.End < initDate)));
            }

            if (!String.IsNullOrEmpty(recharge_type))
                stationsWithPodType = stations.FindAll(s => (s.Pods.Any(p => p.PodType.Name == recharge_type)));

            var stationsQuery = stations.Intersect(stationsWithPodType);

            return PartialView("_StationIndexPartialGrid",stationsQuery.AsQueryable());
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
