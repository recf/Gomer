using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Core
{
    public class PileReport
    {
        public PileReport(Pile pile, DateRange dateRange)
        {
            DateRange = dateRange;

            AddedInPeriod =
                pile.Search(addedOnOrAfter: DateRange.Start, addedBeforeOrOn: DateRange.End).OrderBy(g => g.Name).ToList();

            FinishedInPeriod =
                pile.Search(finishedOnOrAfter: DateRange.Start, finishedBeforeOrOn: DateRange.End, finished: true)
                    .OrderBy(g => g.Name)
                    .ToList();

            var addedCount = AddedInPeriod.Count;
            var finishedCount = FinishedInPeriod.Count;

            Delta = addedCount - finishedCount;
            HoursDelta = AddedHoursInPeriod - FinishedHoursInPeriod;


            Ratio = Reduce(addedCount, finishedCount);
            HoursRatio = Reduce(AddedHoursInPeriod, FinishedHoursInPeriod);
        }

        public int HoursDelta { get; private set; }

        public int Delta { get; private set; }

        public DateRange DateRange { get; private set; }

        public IReadOnlyList<PileGame> AddedInPeriod { get; private set; }

        public int AddedHoursInPeriod
        {
            get { return AddedInPeriod.Sum(g => g.EstimatedHours); }
        }

        public IReadOnlyList<PileGame> FinishedInPeriod { get; private set; }

        public int FinishedHoursInPeriod
        {
            get { return FinishedInPeriod.Sum(g => g.EstimatedHours); }
        }

        public Tuple<decimal, decimal> Ratio { get; private set; }
        public Tuple<decimal, decimal> HoursRatio { get; private set; }

        private Tuple<decimal, decimal> Reduce(int numerator, int demoninator)
        {
            var fence = 10;

            var n = (decimal)numerator;
            var d = (decimal)demoninator;

            var terms = (new[] { 2, 3, 5, 7, 9, 11, 13, 17, 19, 23, 29 }).Reverse().ToArray();

            var index = 0;
            while (index < terms.Length)
            {
                var term = terms[index];

                if (n % term == 0 && d % term == 0)
                {
                    n = n / term;
                    d = d / term;
                }
                else
                {
                    index++;
                }
            }

            if (n > fence)
            {
                d = d/n;
                n = 1;
            }

            return new Tuple<decimal, decimal>(n, d);
        }
    }
}
