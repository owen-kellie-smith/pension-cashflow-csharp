using System;
using System.Collections.Generic;
using PensionModel.Models;

namespace PensionModel.App
{
    public static class ArgsParser
    {
        public static Config Parse(string[] args)
        {
            var config = new Config();  // from App.Config.cs which is POCO with default values
            if (args.Length ==0){
                config.ShowHelp = true;
                return config;
            }

            var dict = DictArgDestinations();  // class defined below

            for (int i = 0; i < args.Length; i++)
            {
                if (!dict.TryGetValue(args[i], out var def)) // def is an ArgSetter defined below
                    continue;   // i.e. just ignore it as a rogue parameter that does no harm

                string? value = null;

                if (def.RequiresValue)
                {
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Out of args. Missing value for {args[i]}");
                    if (dict.TryGetValue(args[i+1], out var def2))
                        throw new ArgumentException($"Next arg recognised as a flag (so it can't be a value). Missing value for {args[i]}");

                    value = args[++i];
                }

                def.Apply(config, value);  // Apply is the Action for args[i] from DictArgDestinations below which mostly sets some Property in config
            }

            return config;
        }

        private static Dictionary<string, ArgSetter> DictArgDestinations() =>
            new(StringComparer.OrdinalIgnoreCase) //   (StringComparer.OrdinalIgnoreCase) means that e.g. --HELP and --help are equivalent
            {
                ["--help"] = new((c, v) => c.ShowHelp = true, false, "List options"), // false here implies value not required i.e. flag only like debug
                ["-h"]     = new((c, v) => c.ShowHelp = true, false, "List options"),
                ["--debug"] = new((c, v) => c.Debug = true, false, "Output interim calcs"),

                ["--mp"]      = new((c, v) => c.MpFile = v, true, "Path to model point file"),
                ["--assets"]  = new((c, v) => c.AssetsFolder = v, true, "Path to assets folder"),
                ["--mort"]    = new((c, v) => c.MortFile = v, true, "Mortality file for single model point"),
                ["--agg"]     = new((c, v) => c.Agg = v, true, "Aggregation type"), // target-typed new which is equivalent to new ArgSetter((c, v) => c.Agg = v, true)
                ["--output"]  = new((c, v) => c.Output = v, true, "Output filename"),

                ["--age"]     = new((c, v) => c.Age = double.Parse(v), true, "Current age in years for single model point"),
                ["--benefit"] = new((c, v) => c.Benefit = double.Parse(v), true, "Benefit amount per year for single model point"),
                ["--rate"]    = new((c, v) => c.Rate = double.Parse(v), true, "Interest rate per year"),
                ["--years"]   = new((c, v) => c.Years = int.Parse(v), true, "Number of projection years"),
            };

        public static string GetHelpText()
        {
            var dict = DictArgDestinations();
            var lines = new List<string>();

            lines.Add("New help Usage:");
            lines.Add("  app [options]");
            lines.Add("");
            lines.Add("Options:");

            foreach (var kvp in dict)
            {
                var name = kvp.Key;
                var def = kvp.Value;

                var valueHint = def.RequiresValue ? " <value>" : "";
                lines.Add($"  {name}{valueHint,-10} {def.HelpText}");
            }

            return string.Join(Environment.NewLine, lines);
        }
            
        private class ArgSetter
        {
            public Action<Config, string> Apply { get; }  // no set i.e. it can only be set in the constructor (write-once)
            public bool RequiresValue { get; }
            public string HelpText { get; }

            public ArgSetter(Action<Config, string> apply, bool requiresValue, string helpText)
            {
                Apply = apply;
                RequiresValue = requiresValue;
                HelpText = helpText;
            }
        }
            
    }
}
