using System;
using System.Collections.Generic;
using System.Linq;
using PensionModel.Models;

namespace PensionModel.Engine
{
    public static class PensionCalculator
    {
        public static List<Cashflow> Calculate(
            List<MortalityRow> mortality,
            ModelPoint mp,
            int years,
            double rate)
        {
            var result = new List<Cashflow>();
            double survival = 1.0;
            var minRow = mortality.First();
            var maxRow = mortality.Last();

            for (int t = 0; t < years; t++)
            {
                double age = mp.AgeAtVDate + t;

                int clampedAge = Math.Clamp((int)age, minRow.Age, maxRow.Age);

                var row = mortality.LastOrDefault(m => m.Age <= clampedAge);
                double qx = row?.Qx ?? 1.0;

                survival *= (1 - qx);

                double cash = mp.BenefitPA * survival;
                double pv = cash * Math.Pow(1 + rate, -(t + 1));

                result.Add(new Cashflow
                {
                    Year = t + 1,
                    CashflowValue = cash,
                    PresentValue = pv
                });
            }

            return result;
        }
    }
}
