namespace MyProject.Tests;
using System.Collections.Generic;
using Xunit;
using PensionModel.Aggregation;
using PensionModel.Models;

public class CashflowAggregatorTests
{
    [Fact]
    public void Aggregate_SumYear_GroupsByYear()
    {
        var input = new List<Cashflow>
        {
            new Cashflow { Year = 1, CashflowValue = 100, PresentValue = 90 },
            new Cashflow { Year = 1, CashflowValue = 50, PresentValue = 40 },
            new Cashflow { Year = 2, CashflowValue = 30, PresentValue = 20 }
        };

        var result = CashflowAggregator.Aggregate(input, "sum_year");

        Assert.Equal(2, result.Count);

        var year1 = result.Find(r => r.Year == 1);
        Assert.Equal(150, year1.CashflowValue);
        Assert.Equal(130, year1.PresentValue);
    }


[Fact]
public void Aggregate_Default_ReturnsSingleRowSum()
{
    var input = new List<Cashflow>
    {
        new Cashflow { Year = 1, CashflowValue = 100, PresentValue = 90 },
        new Cashflow { Year = 2, CashflowValue = 50, PresentValue = 40 }
    };

    var result = CashflowAggregator.Aggregate(input, "anything_else");

    Assert.Single(result);

    Assert.Equal(150, result[0].CashflowValue);
    Assert.Equal(130, result[0].PresentValue);
}
}
