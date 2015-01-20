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
                pile.Search(finishedOnOrAfter: DateRange.Start, finishedBeforeOrOn: DateRange.End)
                    .OrderBy(g => g.Name)
                    .ToList();

            var addedCount = AddedInPeriod.Count;
            var finishedCount = FinishedInPeriod.Count;

            Delta = addedCount - finishedCount;
            HoursDelta = AddedHoursInPeriod - FinishedHoursInPeriod;

            CompletionPercentage = (decimal)finishedCount / addedCount;

            HoursCompletionPercentage = (decimal)FinishedHoursInPeriod / AddedHoursInPeriod;
        }

        public int HoursDelta { get; private set; }

        public int Delta { get; private set; }

        public DateRange DateRange { get; private set; }

        public decimal CompletionPercentage { get; private set; }

        public decimal HoursCompletionPercentage { get; private set; }

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
    }
}
