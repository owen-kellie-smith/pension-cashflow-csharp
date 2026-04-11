using System;
using System.Collections.Generic;

namespace PensionModel.App
{
    public static class ArgsParser
    {
        public static Config Parse(string[] args)
        {
            var config = new Config();  // from App.Config.cs which is POCO with default values
            var dict = DictArgs();

// Source - https://stackoverflow.com/a/16265268
// Posted by Hossein Narimani Rad, modified by community. See post 'Timeline' for change history
// Retrieved 2026-04-11, License - CC BY-SA 4.0
foreach(var item in args)
{
//    Console.WriteLine(item.ToString());
}

            for (int i = 0; i < args.Length; i++)
            {
                if (!dict.TryGetValue(args[i], out var def)) // def is an ArgDef defined below
                    continue;   // i.e. just ignore it as a rogue parameter that does no harm

                string? value = null;

                if (def.RequiresValue)
                {

                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Out of args. Missing value for {args[i]}");
                    if (dict.TryGetValue(args[i+1], out var def2))
                        throw new ArgumentException($"Next arg recognised. Missing value for {args[i]}");

                    value = args[++i];
                }

                def.Apply(config, value);  // Apply is the Action for args[i] from DictArgs below which mostly sets some Property in config
            }

            return config;
        }

        private static Dictionary<string, ArgDef> DictArgs() =>
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["--help"] = new((c, v) => c.ShowHelp = true, false), // false here implies value not required i.e. flag only like debug
                ["-h"]     = new((c, v) => c.ShowHelp = true, false),


                ["--debug"] = new((c, v) => c.Debug = true, false),

                ["--mp"]      = new((c, v) => c.MpFile = v, true),
                ["--assets"]  = new((c, v) => c.AssetsFolder = v, true),
                ["--mort"]    = new((c, v) => c.MortFile = v, true),
                ["--agg"]     = new((c, v) => c.Agg = v, true), // target-typed new which is equivalent to new ArgDef((c, v) => c.Agg = v, true)
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
