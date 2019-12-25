using E_Recarga.Models;
using E_Recarga.Models.ERecargaModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.ViewModels
{
    public class TopStation
    {
        public TopStation()
        {
            TotalProfit = 0;
            DailyProfit = 0;
            WeeklyProfit = 0;
            MonthlyProfit = 0;

            FastChargerUsage = 0;
            NormalChargerUsage = 0;
            TotalSuccessfulChargings = 0;

            AverageChargingTime = 0;
            AverageCost = 0;
            FastChargerUsage = 0;
            NormalChargerUsage = 0;
            InfoDaysOfWeek = new List<DataPoint>();
            HourPlotData = new List<DataPoint>();
        }

        public Station Station { get; set; }

        [Display(Name = "Carregamento Preferido")]
        public PodTypeEnum PreferedPodType { get; set; }

        [Display(Name = "Marcações Concluidas")]
        public int TotalSuccessfulChargings { get; set; }

        [Display(Name = "Total de Marcações")]
        public int TotalAppointments { get; set; }

        [Display(Name = "Receita Total")]
        public double TotalProfit { get; set; }

        [Display(Name = "Receita Diária")]
        public double DailyProfit { get; set; }

        [Display(Name = "Receita Semanal")]
        public double WeeklyProfit { get; set; }

        [Display(Name = "Receita Mensal")]
        public double MonthlyProfit { get; set; }

        [Display(Name = "Horas médias de carregamento")]
        public double AverageChargingTime { get; set; }

        [Display(Name = "Receita Média por Carregamento")]
        public double AverageCost { get; set; }

        [Display(Name = "Carregamentos Rápidos")]
        public int FastChargerUsage { get; set; }

        [Display(Name = "Carregamentos Normais")]
        public int NormalChargerUsage { get; set; }

        public List<DataPoint> InfoDaysOfWeek { get; set; }
        public string InfoDaysJSON { get; set; }

        public List<DataPoint> HourPlotData { get; set; }

        public string HourPlotJSON { get; set; }
    }
}