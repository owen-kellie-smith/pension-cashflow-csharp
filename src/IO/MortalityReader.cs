using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using ExcelDataReader;
using PensionModel.Models;

namespace PensionModel.IO
{
    public static class MortalityParser
    {
        public static List<MortalityRow> Parse(DataTable table)
        {
            var result = new List<MortalityRow>();

            for (int i = 1; i < table.Rows.Count; i++) // skip header
            {
                var row = table.Rows[i];

                var ageObj = row[0];
                var qxObj = row[1];

                if (ageObj == null || qxObj == null)
                    continue;

                int age;
                double qx;

                // handle numeric + string safely
                if (ageObj is int ii)
                    age = ii;
                else if (!int.TryParse(ageObj.ToString(), out age))
                    continue;

                if (qxObj is double dd)
                    qx = dd;
                else if (!double.TryParse(qxObj.ToString(), out qx))
                    continue;
    
                result.Add(new MortalityRow
                {
                    Age = age,
                    Qx = qx
                });
            }

            return result;
        }
    }

    public static class MortalityReader
    {
        public static List<MortalityRow> Read(string file, string folder)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var path = Path.Combine(folder ?? "", file);

            using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);

            var dataset = reader.AsDataSet();
            DataTable table = dataset.Tables[0];

            return MortalityParser.Parse(table);
        }
    }
}
