namespace PensionModel.Models
{
    public class Config
    {
        public double Rate { get; set; } = 0.03;
        public string AssetsFolder { get; set; } = "assets/xls"; // used to locate Mortality tables

        public string? MpFile { get; set; } = null; // MpFile contains per-row definitions of MortFile, Age, Benefit
        public double Age { get; set; } = 65; // so Age is meaningless is there is a MpFile
        public double Benefit { get; set; } = 10000; // and similarly Benefit is meaningless if there is a MpFile
        public string MortFile { get; set; } = "pma92.xls"; // and similarly MortFile is meaningless if there is an MpFile

        public int Years { get; set; } = 10;
        public string Agg { get; set; } = "sum_year";

        public string Output { get; set; } = "out.csv";

        public bool Debug { get; set; } = false;
        public bool ShowHelp { get; set; } = false;
    }
}
