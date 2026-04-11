using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using PensionModel.Models;

namespace PensionModel.IO
{
    public static class ModelPointReader
    {
        public static List<ModelPoint> Load(
            string? mpFile,
            string? mortFile,
            double age,
            double benefit)
        {
            if (mpFile == null)
            {
                return new List<ModelPoint>
                {
                    new ModelPoint
                    {
                        Mortality = mortFile,
                        AgeAtVDate = age,
                        BenefitPA = benefit
                    }
                };
            }

            using var reader = new StreamReader(mpFile);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                IgnoreBlankLines = true,
                PrepareHeaderForMatch = args => args.Header.Trim().ToLower()
            };

            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<ModelPointMap>();

            return new List<ModelPoint>(csv.GetRecords<ModelPoint>());
        }
    }

    public sealed class ModelPointMap : ClassMap<ModelPoint> // selaed class cannot be inherited from
    {
        public ModelPointMap()
        {
            Map(m => m.Mortality).Name("mortality", "mort", "qx");
            Map(m => m.AgeAtVDate).Name("age_at_vdate", "age", "AgeAtVDate");
            Map(m => m.BenefitPA).Name("benefit_pa", "benefit", "BenefitPA");
        }
    }
}
