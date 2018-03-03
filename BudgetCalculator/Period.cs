using System;

namespace BudgetCalculator
{
    internal class Period
    {
        public Period(DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new ArgumentException();
            }

            Start = start;
            End = end;
        }

        public DateTime End { get; private set; }
        public DateTime Start { get; private set; }

        public int EffectiveDays
        {
            get
            {
                return DateTime.DaysInMonth(Start.Year, Start.Month);
            }
        }

        public bool IsSameMonth()
        {
            return Start.Year == End.Year && Start.Month == End.Month;
        }
    }
}