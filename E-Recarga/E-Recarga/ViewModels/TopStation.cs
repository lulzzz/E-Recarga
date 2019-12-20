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
            BestDayOfWeek = new Dictionary<DayOfWeek, double>();
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

        [Display(Name = "Média de Horas de carregamentos")]
        public double AverageChargingTime { get; set; }

        [Display(Name = "Receita Média por Carregamento")]
        public double AverageCost { get; set; }

        [Display(Name = "Melhor Horário")]
        public double BestTimeAvg { get; set; }

        [Display(Name = "Pior Horário")]
        public double WorstTimeAvg { get; set; }

        [Display(Name = "Carregamentos Rápidos")]
        public int FastChargerUsage { get; set; }

        [Display(Name = "Carregamentos Normais")]
        public int NormalChargerUsage { get; set; }

        [Display(Name = "Dias da Semana")]
        public Dictionary<DayOfWeek, double> InfoDaysOfWeek { get; set; }
    }
}