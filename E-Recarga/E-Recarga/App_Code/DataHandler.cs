using E_Recarga.Models;
using E_Recarga.Models.ERecargaModels;
using E_Recarga.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Recarga.App_Code
{
    public class DataHandler
    {
        private static List<TopStation> GetTopStations(int TopSize, List<Station> stations)
        {
            List<TopStation> topStations = new List<TopStation>();

            var orderedStations = stations.OrderBy(s => {
                var totalPrice = s.Appointments.Where(a =>
                                    a.Status.Id == AppointmentStatusEnum.Completed)
                                    .Sum(sum => sum.Cost);
                return totalPrice;
            });

            var enumerator = orderedStations.GetEnumerator();
            for (int i = 0; i < TopSize; i++)
            {
                if (!enumerator.MoveNext())
                {
                    break;
                }
                TopStation Top = new TopStation();
                var Appointments = enumerator.Current.Appointments;

                //Station
                Top.Station = enumerator.Current;

                //Profit
                Top.TotalProfit = Appointments.Sum(a => a.Cost);
                Top.DailyProfit = Appointments
                        .Where(a => a.Start >= DateTime.Now.AddDays(-1) &&
                        a.AppointmentStatusId == AppointmentStatusEnum.Completed)
                        .Sum(a => a.Cost);

                Top.WeeklyProfit = Appointments
                        .Where(a => a.Start >= DateTime.Now.AddDays(-7) &&
                        a.AppointmentStatusId == AppointmentStatusEnum.Completed)
                        .Sum(a=> a.Cost);

                Top.MonthlyProfit = Appointments
                        .Where(a => a.Start >= DateTime.Now.AddMonths(-1) &&
                        a.AppointmentStatusId == AppointmentStatusEnum.Completed)
                        .Sum(a=> a.Cost);

                //Amount of charge cycles
                Top.TotalSuccessfulChargings = Appointments
                        .Where(a => a.AppointmentStatusId == AppointmentStatusEnum.Completed)
                        .Count();

                //Total number of appointments
                Top.TotalAppointments = Appointments.Count;

                //Prefered Pod types
                Top.NormalChargerUsage = Appointments
                        .Where(a => a.PodId == (int?)PodTypeEnum.Normal).Count();

                Top.FastChargerUsage = Top.TotalAppointments - Top.NormalChargerUsage;

                Top.PreferedPodType = Top.FastChargerUsage > Top.NormalChargerUsage ?
                        PodTypeEnum.Fast : PodTypeEnum.Normal;

                //Average Cost of appointments
                Top.AverageCost = Appointments.Average(a => a.Cost);

                Top.AverageChargingTime = Appointments
                        .Select(a => a.End.TimeOfDay - a.Start.TimeOfDay).Average(s => s.Hours);

                //Best and Worst time of day
                var group = Appointments
                        .GroupBy(a => a.Start.TimeOfDay, x => x.End.TimeOfDay - x.Start.TimeOfDay);

                Top.BestTimeAvg = group.Max(g => g.Average(a=> a.Hours));
                Top.WorstTimeAvg = group.Min(g => g.Average(a=> a.Hours));

                //Get best day of the week
                foreach(DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    Top.InfoDaysOfWeek.Add(day, Appointments.Where(a => a.AppointmentStatusId == AppointmentStatusEnum.Completed &&
                       a.Start.DayOfWeek == day).Sum(a => a.Cost));
                }
                Top.InfoDaysOfWeek.OrderBy(v => v.Value);

                topStations.Add(Top);
            }

            return topStations;
        }

        public static DashboardViewModel GetDashboardData(List<Station> stations)
        {
            DashboardViewModel model = new DashboardViewModel();

            model.TopStations = GetTopStations(3, stations);
            model.AverageCost = stations.Average(s => s.Appointments.Average(a => a.Cost));

            model.TotalProfit = stations.Sum(s => s.Appointments.Sum(a => a.Cost));
            model.DailyProfit = stations.Sum(s => s.Appointments
                    .Where(a => a.Start >= DateTime.Now.AddDays(-1) &&
                    a.AppointmentStatusId == AppointmentStatusEnum.Completed)
                    .Sum(a => a.Cost));

            model.WeeklyProfit = stations.Sum(s => s.Appointments
                    .Where(a => a.Start >= DateTime.Now.AddDays(-7) &&
                    a.AppointmentStatusId == AppointmentStatusEnum.Completed)
                    .Sum(a => a.Cost));

            model.MonthlyProfit = stations.Sum(s => s.Appointments
                    .Where(a => a.Start >= DateTime.Now.AddMonths(-1) &&
                    a.AppointmentStatusId == AppointmentStatusEnum.Completed)
                    .Sum(a => a.Cost));

            //Amount of charge cycles
            model.TotalSuccessfulChargings = stations.Sum(s => s.Appointments
                    .Where(a => a.AppointmentStatusId == AppointmentStatusEnum.Completed)
                    .Count());

            //Total number of appointments
            model.TotalAppointments = stations.Sum(s => s.Appointments.Count);

            //Prefered Pod types
            model.NormalChargerUsage = stations.Sum(s => s.Appointments
                    .Where(a => a.PodId == (int?)PodTypeEnum.Normal).Count());

            model.FastChargerUsage = model.TotalAppointments - model.NormalChargerUsage;

            model.PreferedPodType = model.FastChargerUsage > model.NormalChargerUsage ?
                    PodTypeEnum.Fast : PodTypeEnum.Normal;

            //Average Cost of appointments
            model.AverageCost = stations.Average(s => s.Appointments.Average(a => a.Cost));

            model.AverageChargingTime = stations.Average(s => s.Appointments
                    .Select(a => a.End.TimeOfDay - a.Start.TimeOfDay).Average(p => p.Hours));

            //Best and Worst time of day
            var groups = stations.Select(s => s.Appointments
                    .GroupBy(a => a.Start.TimeOfDay, x => x.End.TimeOfDay - x.Start.TimeOfDay));

            model.BestTimeAvg =  groups.Max(gs => gs.Average(g => g.Average(a => a.Hours)));
            model.WorstTimeAvg = groups.Min(gs => gs.Average(g => g.Average(a => a.Hours)));

            //Get best day of the week
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                model.InfoDaysOfWeek.Add(day, stations.Sum(s => s.Appointments
                    .Where(a => a.AppointmentStatusId == AppointmentStatusEnum.Completed &&
                    a.Start.DayOfWeek == day).Sum(a => a.Cost)));
            }
            model.InfoDaysOfWeek.OrderBy(v => v.Value);

            return model;
        }
    }
}