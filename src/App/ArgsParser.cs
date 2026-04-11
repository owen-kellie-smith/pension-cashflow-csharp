using System;
using System.Collections.Generic;

namespace PensionModel.App
{
    public static class ArgsParser
    {
        public static Config Parse(string[] args)
        {
            var config = new Config();  // from App.Config.cs which is POCO with default values
            var map = BuildMap();

            for (int i = 0; i < args.Length; i++)
            {
                if (!map.TryGetValue(args[i], out var def))
                    continue;

                string value = null;

                if (def.RequiresValue)
                {
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Missing value for {args[i]}");

                    value = args[++i];
                }

                def.Apply(config, value);
            }

            return config;
        }

        private static Dictionary<string, ArgDef> BuildMap() =>
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["--help"] = new((c, v) => c.ShowHelp = true, false), // false here implies value not required i.e. flag only lke debug
                ["-h"]     = new((c, v) => c.ShowHelp = true, false),

                ["--debug"] = new((c, v) => c.Debug = true, false),

                ["--mp"]      = new((c, v) => c.MpFile = v, true),
                ["--assets"]  = new((c, v) => c.AssetsFolder = v, true),
                ["--mort"]    = new((c, v) => c.MortFile = v, true),
                ["--agg"]     = new((c, v) => c.Agg = v, true), // target-typed new equivalent to new ArgDef((c, v) => c.Agg = v, true)
                ["--output"]  = new((c, v) => c.Output = v, true),

                ["--age"]     = new((c, v) => c.Age = double.Parse(v), true),
                ["--benefit"] = new((c, v) => c.Benefit = double.Parse(v), true),
                ["--rate"]    = new((c, v) => c.Rate = double.Parse(v), true),
                ["--years"]   = new((c, v) => c.Years = int.Parse(v), true),
            };
            
        private class ArgDef
        {
            public Action<Config, string> Apply { get; }
            public bool RequiresValue { get; }

            public ArgDef(Action<Config, string> apply, bool requiresValue)
            {
                Apply = apply;
                RequiresValue = requiresValue;
            }
        }
            
    }
}
