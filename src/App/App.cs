using System;
using PensionModel.IO;
using PensionModel.Engine;
using PensionModel.Aggregation;
using PensionModel.Models;
using System.Collections.Generic;

namespace PensionModel.App
{
    public static class App
    {
        public static void Run(string[] args)
        {
            var config = ArgsParser.Parse(args);

            if (config.ShowHelp || args.Length == 0)
            {
                PrintHelp();
                return;
            }

            Execute(config);
        }

        private static void Execute(Config config)
        {
            var modelPoints = ModelPointReader.Load(
                config.MpFile,
                config.MortFile,
                config.Age,
                config.Benefit);

            if (config.Debug)
                Console.WriteLine($"Read {modelPoints.Count} model points");

            var cashflows = CashflowService.Build(modelPoints, config);

            var aggregated = CashflowAggregator.Aggregate(cashflows, config.Agg);

            CashflowCsvWriter.Write(aggregated, config.Output);

            if (config.Debug)
                Console.WriteLine($"Processed {modelPoints.Count} model points");
        }

        private static void PrintHelp()
        {
            Console.WriteLine(@"
Pension Cashflow Model

Usage:
  dotnet run -- [options]

Options:
  --mort <file>
  --assets <folder>
  --age <number>
  --benefit <number>
  --years <number>
  --rate <number>
  --mp <file>
  --agg <type>
  --output <file>
  --debug
  --help, -h
");
        }
    }
}
