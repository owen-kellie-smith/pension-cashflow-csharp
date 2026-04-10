namespace MyProject.Tests;
using System.Collections.Generic;
using System.Data;
using Xunit;
using PensionModel.IO;

public class IOTests
{
  [Fact]
  public void Parse_ValidRows_ReturnsMortalityRows()
  {
      var table = new DataTable();
      table.Columns.Add("Age");
      table.Columns.Add("Qx");

      table.Rows.Add("age", "qx");
      table.Rows.Add("10", "0.01");
      table.Rows.Add("20", "0.02");

      var result = MortalityParser.Parse(table);

      Assert.Equal(2, result.Count); // i.e. skipped the top row containing strings age, qx
  }

}
