namespace MyProject.Tests;
using System.Collections.Generic;
using Xunit;
using PensionModel.Engine;
using PensionModel.Models;

public class PensionCalculatorTests
{
    [Fact]
    public void Calculate_ReturnsPositiveCashevenIfMemberBelowMinAgeOfMort()
    {
        var mortality = new List<MortalityRow>
        {
            new MortalityRow { Age = 20, Qx = 0.01 }
        };

        var mp = new ModelPoint
        {
            AgeAtVDate = 10,
            BenefitPA = 1000
        };

        var result = PensionCalculator.Calculate(mortality, mp, years: 5, rate: 0.03);

        Assert.True(result[0].CashflowValue > 0);
    }
    [Fact]
    public void Calculate_ReturnsCorrectNumberOfYears()
    {
        var mortality = new List<MortalityRow>
        {
            new MortalityRow { Age = 20, Qx = 0.01 }
        };

        var mp = new ModelPoint
        {
            AgeAtVDate = 30,
            BenefitPA = 1000
        };

        var result = PensionCalculator.Calculate(mortality, mp, years: 5, rate: 0.03);

        Assert.Equal(5, result.Count);
        Assert.True(result[0].CashflowValue > 0);
    }
}
