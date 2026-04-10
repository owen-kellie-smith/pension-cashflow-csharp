# pension-cashflow-model

![C# Tests](https://github.com/owen-kellie-smith/pension-cashflow-csharp/actions/workflows/dotnet-tests.yml/badge.svg)
[![codecov](https://codecov.io/gh/owen-kellie-smith/pension-cashflow-csharp/graph/badge.svg?token=XLKV69FV53)](https://codecov.io/gh/owen-kellie-smith/pension-cashflow-csharp)

A simple actuarial-style model written in C#.

---

Features:
- reads mortality rates from a standard spreadsheet (.xls)
- reads model points from a model point file (.csv)
- calculates present value of cashflows as benefit amounts * expected survival rate * discount factor
- aggregates results over various indices
- write results to .csv file (optional)

# Getting started

## Prerequisites

- .NET SDK version 8.0+ matching the project's TargetFramework (see `.csproj`)
  https://dotnet.microsoft.com/download

No external packages need to be installed manually.
All dependencies are restored automatically from NuGet.

## Setup

Restore dependencies (optional but useful if you want to be explicit):

```bash
dotnet restore src
dotnet build src

## Run the tests

```bash
dotnet test src/MyProject.Tests/
```
and expect all tests to pass.

## Check test coverage

```bash
dotnet test src/MyProject.Tests/MyProject.Tests.csproj   /p:CollectCoverage=true   /p:CoverletOutputFormat=cobertura
reportgenerator   -reports:**/TestResults/**/coverage.cobertura.xml   -targetdir:coverage-report   -reporttypes:Html
firefox coverage-report/index.html
```
and expect the same degree of coverage as on the badge on GitHub.


## Run the model for a single record specified in the command line

```bash
dotnet run --project src --mort pma92.xls --assets assets/xls --age 65 --benefit 10000 --years 10 --agg sum_year --output o10.csv
cat o10.csv 
```
to get output like
```
    1            9,877.89                9,590.18
    2            9,739.28                9,180.21
    3            9,582.60                8,769.43
    4            9,406.26                8,357.34
    5            9,208.75                7,943.55
    6            8,988.65                7,527.85
    7            8,744.72                7,110.26
    8            8,475.98                6,691.02
    9            8,181.75                6,270.63
   10            7,861.81                5,849.92
```
where the second column are expected amounts of 10,000 paid at the end of each year to survivors age 65 at the start of year 1, and the third columnt are the expected amounts discounted at 3% p.a. E.g. 9,590.18 = 9,877.89 / 1.03, and  9,877.89 = 10,000 * (1 - 0.012211), and 0.012211 = q65 in pma92.xls.

## Run the model for all records specified in a model point file

Aggregate over all indices (i.e. over all projection years and all records) to get a single sum:
```bash
dotnet run --project src --mp assets/csv/MPF.csv --assets assets/xls --age 65 --benefit 10000 --years 10 --agg sum --output oMPF.csv
cat oMPF.csv 
```
to get output like
```
0    452801.3762    388434.6276
```
The 452,801.3762 is the total (for all model points over all 10 projected years) of expected (post-mortality) undiscounted cashflow.

Aggregate over records only (i.e. separate results by projection year)
```bash
dotnet run --project src --mp assets/csv/MPF.csv --assets assets/xls --age 65 --benefit 10000 --years 10 --agg sum_year --output oMPFsum_year.csv
cat oMPFsum_year.csv 
```
to get output like
```
      cashflow  present_value
year                                         
1     49416.800000   47977.475728
2     48756.050944   45957.254165
3     48010.369319   43936.289045
4     47172.368719   41912.038626
5     46234.888304   39882.620796
6     45191.209607   37846.926586
7     44035.349616   35804.768972
8     42762.395085   33757.029561
9     41368.854291   31705.782127
10    39853.090316   29654.441994
```





