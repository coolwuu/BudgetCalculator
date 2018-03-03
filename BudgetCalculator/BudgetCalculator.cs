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
            if (start > end)
            {
                throw new ArgumentException();
            }

            return IsSameMonth(start, end)
                ? GetOneMonthAmount(start, end)
                : GetRangeMonthAmount(start, end);
        }

        private decimal GetRangeMonthAmount(DateTime start, DateTime end)
        {
            var monthCount = end.MonthDifference(start);
            var total = 0;
            for (var index = 0; index <= monthCount; index++)
            {
                if (index == 0)
                {
                    total += GetOneMonthAmount(start, start.LastDate());
                }
                else if (index == monthCount)
                {
                    total += GetOneMonthAmount(end.FirstDate(), end);
                }
                else
                {
                    var now = start.AddMonths(index);
                    total += GetOneMonthAmount(now.FirstDate(), now.LastDate());
                }
            }
            return total;
        }

        private bool IsSameMonth(DateTime start, DateTime end)
        {
            return start.Year == end.Year && start.Month == end.Month;
        }

        private int GetOneMonthAmount(DateTime start, DateTime end)
        {
            var list = this._repo.GetAll();
            var budget = list.Get(start)?.Amount ?? 0;

            var days = DateTime.DaysInMonth(start.Year, start.Month);
            var validDays = GetValidDays(start, end);

            return (budget / days) * validDays;
        }

        private int GetValidDays(DateTime start, DateTime end)
        {
            return (end - start).Days + 1;
        }
    }
}