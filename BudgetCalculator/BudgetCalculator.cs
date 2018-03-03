using System;

namespace BudgetCalculator
{
    internal class BudgetCalculat
    {
        private readonly IRepository<Budget> _repo;

        public BudgetCalculat(IRepository<Budget> repo)
        {
            _repo = repo;
        }

        public decimal Calculate(DateTime start, DateTime end)
        {
            var period = new Period(start, end);

            return period.IsSameMonth()
                ? GetOneMonthAmount(period)
                : GetRangeMonthAmount(start, end, period);
        }

        private int GetOneMonthAmount(Period period)
        {
            var budgets = this._repo.GetAll();
            var budget = budgets.Get(period.Start);

            if (budget == null)
            {
                return 0;
            }

            return budget.DailyAmount() * period.EffectiveDays();
        }

        private decimal GetRangeMonthAmount(DateTime start, DateTime end, Period period)
        {
            //var start = period.Start;
            //var end = period.End;
            var monthCount = end.MonthDifference(start);
            var total = 0;
            for (var index = 0; index <= monthCount; index++)
            {
                if (index == 0)
                {
                    total += GetOneMonthAmount(new Period(start, start.LastDate()));
                }
                else if (index == monthCount)
                {
                    total += GetOneMonthAmount(new Period(end.FirstDate(), end));
                }
                else
                {
                    var now = start.AddMonths(index);
                    total += GetOneMonthAmount(new Period(now.FirstDate(), now.LastDate()));
                }
            }
            return total;
        }
    }
}