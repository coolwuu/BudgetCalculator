using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetCalculator
{
    public static class BudgetExtension
    {
        public static Budget Get(this List<Budget> list, DateTime date)
        {
            return list.FirstOrDefault(r => r.YearMonth == date.ToString("yyyyMM"));
        }
    }
}