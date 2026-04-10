using System.Collections.Generic;
using PensionModel.Models;
using PensionModel.IO;

namespace PensionModel.Engine
{
    public static class CashflowService
    {
        public static List<Cashflow> Build(List<ModelPoint> modelPoints, App.Config config)
        {
            var cache = new Dictionary<string, List<MortalityRow>>();
            var allCashflows = new List<Cashflow>();

            foreach (var mp in modelPoints)
            {
                if (!cache.TryGetValue(mp.Mortality, out var mortality))
                {
                    mortality = MortalityReader.Read(mp.Mortality, config.AssetsFolder);
                    cache[mp.Mortality] = mortality;
                }

                allCashflows.AddRange(
                    PensionCalculator.Calculate(
                        mortality,
                        mp,
                        config.Years,
                        config.Rate));
            }

            return allCashflows;
        }
    }
}
