using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Core
{
    public class PileReport
    {
        private Pile _pile;

        public PileReport(Pile pile, DateTime beginDate, DateTime endDate)
        {
            _pile = pile;
            BeginDate = beginDate;
            EndDate = endDate;
            
            AddedInPeriod =
                pile.Search(addedOnOrAfter: BeginDate, addedBeforeOrOn: EndDate).OrderBy(g => g.Name).ToList();
            
            FinishedInPeriod =
                pile.Search(finishedOnOrAfter: BeginDate, finishedBeforeOrOn: EndDate)
                    .OrderBy(g => g.Name)
                    .ToList();

            var allGames = pile.Search();
            var allFinished = pile.Search(finished: true);

            OverallCount = allGames.Count;

            OverallFinishedCount = allFinished.Count;

            OverallHours = allGames.Sum(g => g.EstimatedHours);
            OverallFinishedHours = allFinished.Sum(g => g.EstimatedHours);
        }

        public DateTime BeginDate { get; private set; }

        public DateTime EndDate { get; private set; }
        
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

        public int OverallFinishedCount { get; private set; }

        public int OverallCount { get; private set; }
        
        public int OverallHours { get; set; }

        public int OverallFinishedHours { get; set; }
    }
}
