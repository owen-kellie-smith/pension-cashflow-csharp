namespace PensionModel.Models
{
    public class ModelPoint
    {
        public string? Mortality { get; set; }
        public double AgeAtVDate { get; set; }
        public double BenefitPA { get; set; }

        public override string ToString()
        {
            return $"Mortality={Mortality}, AgeAtVDate={AgeAtVDate}, BenefitPA={BenefitPA}";
        }
    }
}
