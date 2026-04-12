using System.Collections.Generic;
using System.Linq;
using PensionModel.Models;

namespace PensionModel.Aggregation
{
    public static class CashflowAggregator
    {
        public static List<Cashflow> Aggregate(
            List<Cashflow> cashflows,
            string type)
        {
            if (type == "sum_year")
            {
                return cashflows
                    .GroupBy(c => c.Year)
                    .Select(g => new Cashflow
                    {
                        Year = g.Key,
                        CashflowValue = g.Sum(c => c.CashflowValue),
                        PresentValue = g.Sum(c => c.PresentValue)
                    })
                    .ToList();
            }

            if (type == "sum")
            {
                return cashflows
                    .GroupBy(c => -1)
                    .Select(g => new Cashflow
                    {
                        Year = g.Key,
                        CashflowValue = g.Sum(c => c.CashflowValue),
                        PresentValue = g.Sum(c => c.PresentValue)
                    })
                    .ToList();
            }
            if (true)
            {
                return cashflows
                    .GroupBy(c => -1)
                    .Select(g => new Cashflow
                    {
                        Year = g.Key,
                        CashflowValue = g.Sum(c => c.CashflowValue),
                        PresentValue = g.Sum(c => c.PresentValue)
                    })
                    .ToList();
            }
        }
    }
}
