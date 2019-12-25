using E_Recarga.Models;
using E_Recarga.Models.ERecargaModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.ViewModels
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            TotalProfit = 0;
            DailyProfit = 0;
            WeeklyProfit = 0;
            MonthlyProfit = 0;

            //FastChargerUsage = 0;
            //NormalChargerUsage = 0;
            TotalSuccessfulChargings = 0;

            AverageChargingTime = 0;
            HourlyUsageAvg = 0;
            AverageCost = 0;
            FastChargerUsage = 0;
            NormalChargerUsage = 0;
            InfoDaysOfWeek = new List<DataPoint>();
            InfoPodUsagePerMonth = new List<DataPoint>();
            InfoProfitPerMonth= new List<DataPoint>();
        }

        //Appointments and station general info
        public List<TopStation> TopStations { get; set; }

        [Display(Name = "Carregamento Preferido")]
        public PodTypeEnum PreferedPodType { get; set; }

        [Display(Name = "Marcações Concluidas")]
        public int TotalSuccessfulChargings { get; set; }

        [Display(Name = "Total de Marcações")]
        public int TotalAppointments { get; set; }

        //Profit Section
        [Display(Name = "Receita Total")]
        public double TotalProfit { get; set; }

        [Display(Name = "Receita Diária")]
        public double DailyProfit { get; set; }

        [Display(Name = "Receita Semanal")]
        public double WeeklyProfit { get; set; }

        [Display(Name = "Receita Mensal")]
        public double MonthlyProfit { get; set; }

        //Station Appointments info
        [Display(Name = "Hora de Ponta")]
        public int HourlyUsageAvg { get; set; }

        [Display(Name = "Receita Média por Carregamento")]
        public double AverageCost { get; set; }

        [Display(Name = "Tempo de Carregamento Médio")]
        public double AverageChargingTime { get; set; }

        [Display(Name = "Carregamentos Rápidos")]
        public int FastChargerUsage { get; set; }

        [Display(Name = "Carregamentos Normais")]
        public int NormalChargerUsage { get; set; }

        [Display(Name = "Dias da Semana")]
        public List<DataPoint> InfoDaysOfWeek { get; set; }
        public string InfoDaysOfWeekJSON { get; set; }

        public List<DataPoint> InfoProfitPerMonth { get; set; }
        public string InfoProfitPerMonthJSON { get; set; }

        public List<DataPoint> InfoPodUsagePerMonth { get; set; }
        public string InfoPodUsagePerMonthJSON { get; set; }
    }
}