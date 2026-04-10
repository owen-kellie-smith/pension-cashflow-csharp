namespace PensionModel.App
{
    public class Config
    {
        public string MpFile { get; set; }
        public string AssetsFolder { get; set; }
        public string MortFile { get; set; }
        public string Output { get; set; }

        public double Age { get; set; } = 65;
        public double Benefit { get; set; } = 10000;
        public double Rate { get; set; } = 0.03;
        public int Years { get; set; } = 10;

        public string Agg { get; set; } = "sum";
        public bool Debug { get; set; }
        public bool ShowHelp { get; set; }
    }
}
