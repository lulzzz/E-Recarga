using E_Recarga.Models.ERecargaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Recarga.App_Code
{
    public class ScheduleGenerator
    {
        public static List<Price> GeneratePrices(double normal, double fast)
        {
            List <Price> prices = new List<Price>();
            for (int i = 0; i < 24; i++)
            {
                prices.Add(new Price()
                {
                    Active = true,
                    Time = new TimeSpan(i,0,0),
                    CostNormal = normal,
                    CostUltra = fast
                });
            }

            return prices;
        }

    }
}