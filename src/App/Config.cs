namespace PensionModel.App
{
    public class Config
    {
        public string MpFile { get; set; }
        public string AssetsFolder { get; set; } = "assets/xls";
        public string MortFile { get; set; } = "pma92.xls";
        public string Output { get; set; } = "out.csv";

        public double Age { get; set; } = 65;
        public double Benefit { get; set; } = 10000;
        public double Rate { get; set; } = 0.03;
        public int Years { get; set; } = 10;

        public string Agg { get; set; } = "sum_year";
        public bool Debug { get; set; }
        public bool ShowHelp { get; set; }
    }
}
