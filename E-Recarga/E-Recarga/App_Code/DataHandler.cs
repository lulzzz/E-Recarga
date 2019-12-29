using E_Recarga.Models;
using E_Recarga.Models.ERecargaModels;
using E_Recarga.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace E_Recarga.App_Code
{
    public class DataHandler
    {

        private static JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

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
                Top.TotalProfit = Math.Round(Appointments.Sum(a => a.Cost),2);
                Top.DailyProfit = Math.Round(Appointments
                        .Where(a => a.Start >= DateTime.Now.AddDays(-1) &&
                        a.AppointmentStatusId == AppointmentStatusEnum.Completed)
                        .Sum(a => a.Cost),2);

                Top.WeeklyProfit = Math.Round(Appointments
                        .Where(a => a.Start >= DateTime.Now.AddDays(-7) &&
                        a.AppointmentStatusId == AppointmentStatusEnum.Completed)
                        .Sum(a=> a.Cost),2);

                Top.MonthlyProfit = Math.Round(Appointments
                        .Where(a => a.Start >= DateTime.Now.AddMonths(-1) &&
                        a.AppointmentStatusId == AppointmentStatusEnum.Completed)
                        .Sum(a=> a.Cost),2);

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
                Top.AverageCost = Math.Round(Appointments.Average(a => a.Cost),2);

                Top.AverageChargingTime = Math.Round(Appointments
                        .Select(a => a.End - a.Start).Average(s => s.Hours),2);

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

                        //if (start <= timeStart && end >= timeStart ||
                        //        start <= timeEnd && end >= timeEnd)

                        if (start <= timeStart && end >= timeEnd)
                            return true;
                        else
                            return false;
                    }).Count();

                    if(Top.Station.ComercialName == "Station C")
                    {}
                    Top.HourPlotData.Add(new DataPoint(quantity,timeStart.ToString("HH:mm")));

                    ts = new TimeSpan(x+1,0,0);
                }

                //Get best day of the week
                foreach(DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    var value = Appointments.Where(a => a.AppointmentStatusId == AppointmentStatusEnum.Completed &&
                        a.Start >= DateTime.Now.AddMonths(-1) &&
                        a.Start.DayOfWeek == day).Sum(a => a.Cost);

                    if(!(value < 1))
                    {
                        Top.InfoDaysOfWeek.Add(new DataPoint(value, Enum.GetName(typeof(DayOfWeek), day)));
                    }

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

            model.AverageCost = Math.Round(stations.Average(s => s.Appointments.Count > 0 ? s.Appointments.Average(a => a.Cost) : 0),2);

            model.TotalProfit = Math.Round(stations.Sum(s => s.Appointments.Sum(a => a.Cost)),2);
            model.DailyProfit = Math.Round(stations.Sum(s => s.Appointments
                    .Where(a => a.Start >= DateTime.Now.AddDays(-1) &&
                    a.AppointmentStatusId == AppointmentStatusEnum.Completed)
                    .Sum(a => a.Cost)),2);

            model.WeeklyProfit = Math.Round(stations.Sum(s => s.Appointments
                    .Where(a => a.Start >= DateTime.Now.AddDays(-7) &&
                    a.AppointmentStatusId == AppointmentStatusEnum.Completed)
                    .Sum(a => a.Cost)),2);

            model.MonthlyProfit = Math.Round(stations.Sum(s => s.Appointments
                    .Where(a => a.Start >= DateTime.Now.AddMonths(-1) &&
                    a.AppointmentStatusId == AppointmentStatusEnum.Completed)
                    .Sum(a => a.Cost)),2);

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
            model.AverageCost = Math.Round(stations.Average(s => s.Appointments.Count > 0 ? s.Appointments.Average(a => a.Cost) : 0),2);

            model.AverageChargingTime = Math.Round(stations.Average(s => s.Appointments.Count > 0 ?
                    s.Appointments.Select(a => a.End - a.Start).Average(p => p.Hours) : 0),2);

            //Get days
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                var sum = stations.Sum(s => s.Appointments.Where(a =>
                    a.AppointmentStatusId == AppointmentStatusEnum.Completed &&
                    a.Start >= DateTime.Now.AddMonths(-1) &&
                    a.Start.DayOfWeek == day).Sum(a => a.Cost));

                if(!(sum < 1))
                {
                    model.InfoDaysOfWeek.Add(new DataPoint(Math.Round(sum,2), Enum.GetName(typeof(DayOfWeek), day)));
                }
            }

            //Get monthly profit and montly pod usage
            var pt = CultureInfo.CreateSpecificCulture("pt-PT").DateTimeFormat;
            var Months = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList();
            Months.RemoveAt(Months.Count - 1);


            //Cycle goes backwards to keep month/year order on the view
            for(int i = 12; i >= 0; i--)
            {
                DateTime temp = DateTime.Now.AddMonths(-i);

                var profit = Math.Round(stations.Sum(s => s.Appointments.Where(a =>
                    a.Start.Year == temp.Year &&
                    a.Start.Month == temp.Month).Sum(a => a.Cost)),2);

                model.InfoProfitPerMonth.Add(new DataPoint(profit, Months.ElementAt(temp.Month - 1) + "-" + temp.Year));
            }


            foreach (var top in model.TopStations)
            {
                top.HourPlotJSON = JsonConvert.SerializeObject(top.HourPlotData, _jsonSetting);
                top.InfoDaysJSON = JsonConvert.SerializeObject(top.InfoDaysOfWeek, _jsonSetting);
            }

            model.InfoDaysOfWeekJSON = JsonConvert.SerializeObject(model.InfoDaysOfWeek, _jsonSetting);
            model.InfoPodUsagePerMonthJSON = JsonConvert.SerializeObject(model.InfoPodUsagePerMonth, _jsonSetting);
            model.InfoProfitPerMonthJSON = JsonConvert.SerializeObject(model.InfoProfitPerMonth, _jsonSetting);

            return model;
        } 
    }
}