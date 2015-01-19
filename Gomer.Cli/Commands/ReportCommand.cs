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
        private DateRange _shortTerm;

        private bool _useBBCode;

        private bool _showList;
        private DateRange _longTerm;

        public ReportCommand()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfYear = new DateTime(today.Year, 1, 1);

            _shortTerm = new DateRange(startOfMonth, startOfMonth.AddMonths(1).AddDays(-1));

            _longTerm = new DateRange(startOfYear, startOfYear.AddYears(1).AddDays(-1));

            _useBBCode = false;
            _showList = false;

            IsCommand("report", "Show report of changes for a date range.");

            Arg(
                "short",
                "Short term date {{RANGE}} to report on. (default: {0})",
                DateRange.Parse,
                v => _shortTerm = v,
                's',
                _shortTerm);

            Arg(
                "long",
                "Long term date {{RANGE}} to report on. (default: {0})",
                DateRange.Parse,
                v => _longTerm = v,
                'l',
                _longTerm);

            Flag("bbcode", "Output in BBCode format, for posting in forums.", v => _useBBCode = v);

            Flag("list", "List the games in the report, in addition to stats.", v => _showList = v);
        }

        public override void Run(string[] remainingArguments, TextWriter output)
        {
            var pile = ReadFile();
            var shortTermReport = new PileReport(pile, _shortTerm);
            var longTermReport = new PileReport(pile, _longTerm);

            var shortTermStats = MakeStatsDictionary(shortTermReport);
            var longTermStats = MakeStatsDictionary(longTermReport);

            var lists = new Dictionary<string, IReadOnlyList<PileGame>>
            {
                { "Added", shortTermReport.AddedInPeriod },
                { "Finished", shortTermReport.FinishedInPeriod }
            };

            if (_useBBCode)
            {
                OutputBbCode(null, shortTermStats, lists, output);
                OutputBbCode("Long Term", longTermStats, null, output);
            }
            else
            {
                OutputConsole(null, shortTermStats, lists, output);
                OutputConsole("Long Term", longTermStats, null, output);
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
                
                { "Ratio", string.Format("{0}:{1}", report.Ratio.Item1, report.Ratio.Item2) },
                { "Hours Ratio", string.Format("{0}:{1}", report.HoursRatio.Item1, report.HoursRatio.Item2) }
            };
        }

        private void OutputConsole(string label, Dictionary<string, string> stats, Dictionary<string, IReadOnlyList<PileGame>> lists, TextWriter output)
        {
            output.WriteLine();

            if (!string.IsNullOrWhiteSpace(label))
            {
                output.WriteLine(label);
                output.WriteLine(new string('=', label.Length));
                output.WriteLine();
            }

            var labelWidth = stats.Max(kvp => kvp.Key.Length);
            var valueWidth = stats.Max(kvp => kvp.Value.Length);
            var statFormat = string.Format("{{0,-{0}}}: {{1,{1}}}", labelWidth, valueWidth);

            foreach (var kvp in stats)
            {
                output.WriteLine(statFormat, kvp.Key, kvp.Value);
            }

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

        private void OutputBbCode(string label, Dictionary<string, string> stats, Dictionary<string, IReadOnlyList<PileGame>> lists, TextWriter output)
        {
            output.WriteLine();

            if (!string.IsNullOrWhiteSpace(label))
            {
                output.WriteLine("[b]{0}[/b]", label);
                output.WriteLine();
            }

            var labelWidth = stats.Max(kvp => kvp.Key.Length);
            var valueWidth = stats.Max(kvp => kvp.Value.Length);
            var statFormat = string.Format("[tr][td][b] {{0,-{0}}} [/b][/td][td] {{1,{1}}} [/td][/tr]", labelWidth, valueWidth);

            output.WriteLine("[table]");
            foreach (var kvp in stats)
            {
                output.WriteLine(statFormat, kvp.Key, kvp.Value);
            }
            output.WriteLine("[/table]");

            if (!_showList || lists == null)
            {
                return;
            }


            const string itemFormat = "[*] {0} ({1})";
            foreach (var kvp in lists)
            {
                output.WriteLine();
                output.WriteLine("[b]{0}[/b]", kvp.Key);

                output.WriteLine();
                output.WriteLine("[list]");
                foreach (var game in kvp.Value)
                {
                    output.WriteLine(itemFormat, game.Name, game.Platform);
                }
                output.WriteLine("[/list]");
            }
        }
    }
}
