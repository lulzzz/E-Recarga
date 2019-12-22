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

            var orderedStations = stations.OrderByDescending(s => {
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

                if (Appointments.Count <= 0)
                {
                    topStations.Add(Top);
                    continue;
                }

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

                //Top.AverageChargingTime = Appointments
                //        .Select(a => a.End - a.Start).Average(s => s.Hours);

                double sum = 0;
                foreach (var ap in Appointments)
                {
                    sum += (ap.End - ap.Start).Hours;
                }
                Top.AverageChargingTime = sum;


                TimeSpan ts = new TimeSpan(0, 0, 0);
                DateTime timeStart = new DateTime();
                for (int x = 0; x < 24; x++)
                {
                    var quantity = Appointments.Where(a =>
                    {
                        DateTime start = a.Start.Date.AddHours(a.Start.Hour);
                        DateTime end = a.End.Date.AddHours(a.End.Hour);
                        timeStart = a.Start.Date + ts;
                        DateTime timeEnd = a.End.Date + ts;
                        if (start <= timeStart && end >= timeStart ||
                                start <= timeEnd && end >= timeEnd)
                            return true;
                        else
                            return false;
                    }).Count();
                    Top.HourPlotData.Add(new DataPoint(quantity,timeStart.ToString("HH:mm")));
                    ts = new TimeSpan(x+1,0,0);
                }

                //Get best day of the week
                foreach(DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    var value = Appointments.Where(a => a.AppointmentStatusId == AppointmentStatusEnum.Completed &&
                       a.Start.DayOfWeek == day).Sum(a => a.Cost);

                    Top.InfoDaysOfWeek.Add(new DataPoint(value,nameof(day)));

                }

                topStations.Add(Top);
            }

            return topStations;
        }

        public static DashboardViewModel GetDashboardData(List<Station> stations)
        {
            DashboardViewModel model = new DashboardViewModel();

            if (stations.Count <= 0)
                return model;

            model.TopStations = GetTopStations(3, stations);

            model.AverageCost = stations.Average(s => s.Appointments.Count > 0? s.Appointments.Average(a => a.Cost): 0);

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
            model.AverageCost = stations.Average(s => s.Appointments.Count > 0? s.Appointments.Average(a => a.Cost) : 0);

            model.AverageChargingTime = stations.Average(s => s.Appointments.Count > 0?
                    s.Appointments.Select(a => a.End - a.Start).Average(p => p.Hours): 0);

            //Get best day of the week
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                var sum = stations.Sum(s => s.Appointments
                    .Where(a => a.AppointmentStatusId == AppointmentStatusEnum.Completed &&
                    a.Start.DayOfWeek == day).Sum(a => a.Cost));

                model.InfoDaysOfWeek.Add(new DataPoint(sum,nameof(day)));
            }

            return model;
        }
    }
}