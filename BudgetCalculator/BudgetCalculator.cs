using System;
using System.Collections.Generic;

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

            var budget = this._repo.GetAll().Get(period.Start);
            return period.IsSameMonth()
                ? GetOneMonthAmount(period, budget)
                : GetRangeMonthAmount(period);
        }

        private int GetOneMonthAmount(Period period, Budget budget)
        {
            if (budget == null)
            {
                return 0;
            }
            return budget.DailyAmount() * period.EffectiveDays();
        }

        private decimal GetRangeMonthAmount(Period period)
        {
            var monthCount = period.MonthCount();
            var total = 0;

            for (var index = 0; index <= monthCount; index++)
            {
                var effectivePeriod = GetEffectivePeriod(period, index, monthCount);
                total += GetOneMonthAmount(effectivePeriod, this._repo.GetAll().Get(effectivePeriod.Start));
            }
            return total;
        }

        private static Period GetEffectivePeriod(Period period, int index, int monthCount)
        {
            Period effectivePeriod;
            if (index == 0)
            {
                effectivePeriod = new Period(period.Start, period.Start.LastDate());
            }
            else if (index == monthCount)
            {
                effectivePeriod = new Period(period.End.FirstDate(), period.End);
            }
            else
            {
                effectivePeriod = new Period(period.Start.AddMonths(index).FirstDate(),
                    period.Start.AddMonths(index).LastDate());
            }
            return effectivePeriod;
        }
    }
}