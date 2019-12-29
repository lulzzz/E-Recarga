using E_Recarga.Models;
using E_Recarga.Models.ERecargaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Recarga.App_Code
{
    public class PriceGenerator
    {
        public static double CalculatePrice(DateTime init, DateTime end, IList<Price> pricesList, PodTypeEnum type)
        {
            double price, finalPrice = 0.0;
            int initH = init.Hour, endH = end.Hour;

            price = pricesList.Where(l => l.Time.Hours == initH)
                .Select(l => type == PodTypeEnum. Normal? l.CostNormal : l.CostUltra)
                .FirstOrDefault();

            finalPrice += (60 - init.Minute) * price / 60.0;

            if(initH != endH)
            {
                price = pricesList.Where(l => l.Time.Hours == endH)
                    .Select(l => type == PodTypeEnum.Normal ? l.CostNormal : l.CostUltra)
                    .FirstOrDefault();

                finalPrice += (60 - end.Minute) * price / 60.0;

                initH++;

                while (initH < endH)
                {
                    finalPrice += pricesList.Where(l => l.Time.Hours == initH)
                        .Select(l => type == PodTypeEnum.Normal ? l.CostNormal : l.CostUltra)
                        .FirstOrDefault();
                    initH++;
                }
            }

            return Math.Round(finalPrice,2);
        }
    }
}