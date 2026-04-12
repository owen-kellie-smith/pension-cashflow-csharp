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
          var dict = DictGrouping();

          if (!dict.TryGetValue(type, out var keySelector))
          {
              keySelector = dict["default"];
          }

          return cashflows
              .GroupBy(keySelector)
              .Select(g => new Cashflow
              {
                  Year = g.Key,
                  CashflowValue = g.Sum(c => c.CashflowValue),
                  PresentValue = g.Sum(c => c.PresentValue)
              })
              .ToList();   // g is a group selected by .GroupBy(keySelector). g.key is the grouping key (year or 0 or -99). g.Sum loops over all items in the group.  If grouping key is constant say 0 or -99 then it's the same key for all cashflows and so all cashflows are in a single group.
      }

      // Func<Cashflow, int> means a function that takes one Cahflow argument and returns an int
      private static Dictionary<string, Func<Cashflow, int>> DictGrouping() => 
          new(StringComparer.OrdinalIgnoreCase)
          {
              ["sum_year"] = c => c.Year,
              ["sum"]      = c => 0,
              ["default"]  = c => -99
          };
  }

}
