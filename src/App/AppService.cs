using System;
using System.Collections.Generic;
using System.Linq;
using PensionModel.IO;
using PensionModel.Models;
using PensionModel.Engine;
using PensionModel.Aggregation;

namespace PensionModel.App
{
    public static class AppService
    {
        public static void Run(string[] args)
        {
    	    var config = ArgsParser.Parse(args);
    	    
    	    if (config.ShowHelp){
                Console.WriteLine(ArgsParser.GetHelpText());
                return;
            }
                
            var modelPoints = ModelPointReader.Load(config.MpFile, config.MortFile, config.Age, config.Benefit);
            // -----------------------------
            // 1. Build mortality cache
            // -----------------------------
            var mortalityCache = new Dictionary<string, List<MortalityRow>>();

            var requiredTables = modelPoints
                .Select(mp => mp.Mortality)
                .Distinct();

            foreach (var tableName in requiredTables)
            {
                if (!mortalityCache.ContainsKey(tableName))
                {
                    var rows = MortalityReader.Read(tableName, config.AssetsFolder);
                    mortalityCache[tableName] = rows;
                }
            }

            // -----------------------------
            // 2. Call Engine
            // -----------------------------
            var cashflows = CashflowService.Build(
                modelPoints,
                mortalityCache,
                config
            );

            var aggregated = CashflowAggregator.Aggregate(cashflows, config.Agg);

            // -----------------------------
            // 3. Output (simple for now)
            // -----------------------------

            CashflowCsvWriter.Write(aggregated, config.Output);
            Console.WriteLine($"Wrote {cashflows.Count} cashflows to {config.Output}");
        }
        
    }
}
