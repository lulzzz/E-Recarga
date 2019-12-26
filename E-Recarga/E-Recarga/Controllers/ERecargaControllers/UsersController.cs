using E_Recarga.Models;
using E_Recarga.Models.ERecargaModels;
using E_Recarga.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace E_Recarga.Controllers.ERecargaControllers
{
    [Authorize(Roles = nameof(RoleEnum.User))]
    public class UsersController : Controller
    {
        private ERecargaDbContext db = new ERecargaDbContext();

        // GET: Users
        public ActionResult Index()
        {
            var stations = db.Stations;

            UserViewModel userVM = new UserViewModel();
            userVM.Stations = new List<Station>();
            userVM.EndCharge = DateTime.Now;
            userVM.InitCharge = DateTime.Now;
            ViewBag.Regions = stations.Select(s => s.Region).Distinct().ToList();
            ViewBag.PodTypes = db.Pods.Select(p=>(p.PodType).Name).Distinct().ToList();

            return View(userVM);
        }

        [HttpPost]
        public ViewResult Index([Bind(Include = "Region,PodType,InitCharge,EndCharge")] UserViewModel userVM)
        {
            if (string.IsNullOrEmpty(userVM.Region) || string.IsNullOrEmpty(userVM.PodType) ||
                userVM.InitCharge < DateTime.Now || userVM.EndCharge < userVM.InitCharge)
            {
                userVM.Stations = new List<Station>();
                return View(userVM);
            }

            var stations = db.Stations.Where(s => s.Region.ToLower().Contains(userVM.Region.ToLower())).ToList();
            stations = stations.FindAll(s => s.Pods.Any(p => p.PodType.Name == userVM.PodType));
            userVM.Stations = stations
                    .FindAll(s => s.Appointments
                     .Any(a => a.Start > userVM.EndCharge || a.End < userVM.InitCharge));

            ViewBag.Regions = db.Stations.Select(s => s.Region).Distinct().ToList();
            ViewBag.PodTypes = db.Pods.Select(p => (p.PodType).Name).Distinct().ToList();

            return View(userVM);
        }

        [ChildActionOnly]
        public PartialViewResult IndexGrid(List<Station> Stations)
        {
            Stations = Stations ?? new List<Station>();
            return PartialView("_StationIndexPartialGrid", Stations.AsQueryable());
        }

        public ActionResult AddMoney()
        {
            string userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            user.Wallet = user.Wallet;
            return View(new AddMoneyViewModel() { Name = user.Name, Wallet = user.Wallet, Input = 0 });
        }

        [HttpPost]
        public ActionResult AddMoney([Bind(Include ="Name,Wallet,Input")]AddMoneyViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);

                user.Wallet += userVM.Input;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("AddMoney");
            }

            return View(userVM);
        }
    }
}
