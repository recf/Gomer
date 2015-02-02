using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Core;

namespace Gomer.Cli.Commands
{
    class ReportCommand : BaseCommand
    {
        private DateRange _dateRange;

        private bool _useBBCode;

        private bool _showList;

        public ReportCommand()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfYear = new DateTime(today.Year, 1, 1);

            var thisMonth = new DateRange(startOfMonth, startOfMonth.AddMonths(1).AddDays(-1));
            var lastMonth = new DateRange(startOfMonth.AddMonths(-1), startOfMonth.AddDays(-1));
            var yearToDate = new DateRange(startOfYear, null);

            _dateRange = thisMonth;

            _useBBCode = false;
            _showList = false;

            IsCommand("report", "Show report of changes for a date range.");

            Arg(
                "date-range",
                "Date {{RANGE}} to report on. (default: {0})",
                DateRange.Parse,
                v => _dateRange = v,
                'd',
                _dateRange);

            Flag(
                "last-month",
                string.Format("Show report for last month. Equivalent to --date-range {0}", lastMonth), 
                v => _dateRange = lastMonth);

            Flag(
                "ytd",
                string.Format("Show report for year-to-date. Equivalent to --date-range {0}", yearToDate),
                v => _dateRange = yearToDate);

            Flag("bbcode", "Output in BBCode format, for posting in forums.", v => _useBBCode = v);

            Flag("list", "List the games in the report, in addition to stats.", v => _showList = v);
        }

        public override void Run(string[] remainingArguments, TextWriter output)
        {
            var pile = ReadFile();
            var report = new PileReport(pile, _dateRange);

            var shortTermStats = MakeStatsDictionary(report);

            var lists = new Dictionary<string, IReadOnlyList<PileGame>>
            {
                { "Added", report.AddedInPeriod },
                { "Finished", report.FinishedInPeriod }
            };

            if (_useBBCode)
            {
                OutputBbCode(report.DateRange, shortTermStats, lists, output);
            }
            else
            {
                OutputConsole(report.DateRange, shortTermStats, lists, output);
            }
        }

        private static Dictionary<string, string> MakeStatsDictionary(PileReport report)
        {
            return new Dictionary<string, string>
            {
                { "Added", report.AddedInPeriod.Count.ToString() },
                { "Finished", report.FinishedInPeriod.Count.ToString() },
                { "Delta", report.Delta.ToString() },

                { "Added hours", report.AddedHoursInPeriod.ToString() },
                { "Finished hours", report.FinishedHoursInPeriod.ToString() },
                { "Hours Delta", report.HoursDelta.ToString() },
                
                { "Ratio", string.Format("{0:G2}:{1:G2}", report.Ratio.Item1, report.Ratio.Item2) },
                { "Hours Ratio", string.Format("{0:G2}:{1:G2}", report.HoursRatio.Item1, report.HoursRatio.Item2) }
            };
        }

        private void OutputConsole(DateRange dateRange, Dictionary<string, string> stats, Dictionary<string, IReadOnlyList<PileGame>> lists, TextWriter output)
        {
            output.WriteLine();

            var header = GetHeader(dateRange);
            output.WriteLine(header);
            output.WriteLine(new string('=', header.Length));
            output.WriteLine();

            var labelWidth = stats.Max(kvp => kvp.Key.Length);
            var valueWidth = stats.Max(kvp => kvp.Value.Length);
            var statFormat = string.Format("{{0,-{0}}}: {{1,{1}}}", labelWidth, valueWidth);

            foreach (var kvp in stats)
            {
                output.WriteLine(statFormat, kvp.Key, kvp.Value);
            }

            // TODO: Move Console list output to a separate method
            if (!_showList || lists == null)
            {
                return;
            }

            const string itemFormat = "{0} ({1})";
            foreach (var kvp in lists)
            {
                output.WriteLine();
                output.WriteLine(kvp.Key);

                output.WriteLine(new string('=', kvp.Key.Length));
                output.WriteLine();
                foreach (var game in kvp.Value)
                {
                    output.WriteLine(itemFormat, game.Name, game.Platform);
                }
            }
        }

        private void OutputBbCode(DateRange dateRange, Dictionary<string, string> stats, Dictionary<string, IReadOnlyList<PileGame>> lists, TextWriter output)
        {
            output.WriteLine();

            var header = GetHeader(dateRange);
            output.WriteLine("[b]{0}[/b]", header);
            output.WriteLine();

            var statFormat = "{0}: {1}";

            foreach (var kvp in stats)
            {
                output.WriteLine(statFormat, kvp.Key, kvp.Value);
            }

            // TODO: Move BBCode list output to a separate method
            if (!_showList || lists == null)
            {
                return;
            }

            const string itemFormat = "[*] {0} ({1})";
            foreach (var kvp in lists)
            {
                output.WriteLine();
                output.WriteLine("[i]{0}[/i]", kvp.Key);

                output.WriteLine();
                output.WriteLine("[list]");
                foreach (var game in kvp.Value)
                {
                    output.WriteLine(itemFormat, game.Name, game.Platform);
                }
                output.WriteLine("[/list]");
            }
        }

        private static string GetHeader(DateRange dateRange)
        {
            if (dateRange.Start.HasValue
                && dateRange.End.HasValue
                && dateRange.Start.Value.Day == 1
                && dateRange.End.Value == dateRange.Start.Value.AddMonths(1).AddDays(-1))
            {
                return dateRange.Start.Value.ToString("MMMM yyyy");
            }

            var yearStart = new DateTime(DateTime.Today.Year, 1, 1);
            var yearEnd = yearStart.AddYears(1).AddDays(-1);
            if (dateRange.Start.HasValue
                && dateRange.Start.Value == yearStart
                && (dateRange.End ?? yearEnd) == yearEnd)
            {
                return string.Format("{0:yyyy} Year to Date", dateRange.Start.Value);
            }

            if (!dateRange.Start.HasValue && !dateRange.End.HasValue)
            {
                return "All time";
            }

            return string.Format("{0}", dateRange);
        }
    }
}
