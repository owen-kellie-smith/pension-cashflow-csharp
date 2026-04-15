using System.Collections.Generic;
using PensionModel.Models;


namespace PensionModel.Engine
{
    public static class CashflowService
    {
	public static List<Cashflow> Build(
    		List<ModelPoint> modelPoints,
    		Dictionary<string, List<MortalityRow>> mortalityCache,
    		Config config)
        {
            var allCashflows = new List<Cashflow>();

            foreach (var mp in modelPoints)
            {
                if (config.Debug){
                    Console.WriteLine( mp.ToString() );
                }

    		var mortality = mortalityCache[mp.Mortality];

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
