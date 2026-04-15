namespace MyProject.Tests;
using System.Collections.Generic;
using System.Data;
using Xunit;
using PensionModel.App;

public class ArgsParserTests
{
    [Fact]
    public void arg_as_expected()
    {
        // Arrange

        var args = new[] { "--someDudArgument", "someDudValue", "--someDudFlag", "--mp", "a_file.csv", "--debug", "--agg", "SomeAgg", "--assets", "someFolder", "--mort", "someMortFile", "--age", "99", "--benefit", "12345.67", "--rate", "3.14", "--years", "123", "--output", "someOutFile.csv" };
        // Act
        var config = ArgsParser.Parse(args);
           
        // Assert
        Assert.Equal("a_file.csv", config.MpFile);
        Assert.Equal(99, config.Age);
        Assert.Equal(12345.67, config.Benefit);
        Assert.Equal(3.14, config.Rate);
        Assert.Equal(123, config.Years);
    }
    [Fact]
    public void Missing_arg_throws_error()
    {
        // Arrange
        var args = new[] { "--mp", "--debug" };

        // Assert throws exception
        Assert.Throws<ArgumentException>(() => ArgsParser.Parse(args));

        args = new[] { "--debug", "--mp" };

        // Assert throws exception
        Assert.Throws<ArgumentException>(() => ArgsParser.Parse(args));
    }
    [Fact]
    public void help_file_shown_without_error()
    {
        var args = new[] {  "-h",  };
        AppService.Run(args);
    }
    [Fact]
    public void minimal_input_runs_without_error()
    {
        var args = new[] { "--debug", "--assets", "../../../../../assets/xls"  };
        AppService.Run(args);
    }
    [Fact]
    public void mpf_input_runs_without_error()
    {
        var args = new[] { "--debug", "--assets", "../../../../../assets/xls", "--mp", "../../../../../assets/csv/MPF.csv"  };
        AppService.Run(args);
    }
}
