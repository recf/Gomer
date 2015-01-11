using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Core;

namespace Gomer.Cli.Commands
{
    class ReportCommand : BaseCommand
    {
        private DateTime _beginDate;

        private DateTime _endDate;

        private bool _useBBCode;

        private bool _showList;

        public ReportCommand()
        {
            var today = DateTime.Today;
            _beginDate = today.AddDays(-today.Day + 1);
            _endDate = _beginDate.AddMonths(1).AddDays(-1);
            _useBBCode = false;
            _showList = false;

            IsCommand("report", "Show report of changes for a date range.");

            Arg(
                "begin",
                "Begin {{DATE}} of the date range. (default: {0:})",
                v => _beginDate = v,
                'b',
                _beginDate);

            Arg(
                "end",
                "End {{DATE}} of the date range. (default: {0})",
                v => _endDate = v,
                'e',
                _endDate);

            Flag("bbcode", "Output in BBCode format, for posting in forums.", v => _useBBCode = v);

            Flag("list", "List the games in the report, in addition to stats.", v => _showList = v);
        }

        public override void Run(string[] remainingArguments, TextWriter output)
        {
            var pile = ReadFile();
            var report = new PileReport(pile, _beginDate, _endDate);

            var stats = new Dictionary<string, int>
            {
                { "Added", report.AddedInPeriod.Count },
                { "Finished", report.FinishedInPeriod.Count },
                { "Added hours", report.AddedHoursInPeriod },
                { "Finished hours", report.FinishedHoursInPeriod },
                { "Total", report.OverallCount },
                { "Total Finished", report.OverallFinishedCount },
                { "Total Hours", report.OverallHours },
                { "Total Finished Hours", report.OverallFinishedHours }
            };

            var lists = new Dictionary<string, IReadOnlyList<PileGame>>
            {
                { "Added", report.AddedInPeriod },
                { "Finished", report.FinishedInPeriod }
            };

            if (_useBBCode)
            {
                OutputBbCode(stats, lists, output);
            }
            else
            {
                OutputConsole(stats, lists, output);
            }
        }

        private void OutputConsole(Dictionary<string, int> stats, Dictionary<string, IReadOnlyList<PileGame>> lists, TextWriter output)
        {
            var labelWidth = stats.Max(kvp => kvp.Key.Length);
            var valueWidth = stats.Max(kvp => kvp.Value.ToString().Length);
            var statFormat = string.Format("{{0,-{0}}}: {{1,{1}}}", labelWidth, valueWidth);

            foreach (var kvp in stats)
            {
                output.WriteLine(statFormat, kvp.Key, kvp.Value);
            }

            if (!_showList)
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

        private void OutputBbCode(Dictionary<string, int> stats, Dictionary<string, IReadOnlyList<PileGame>> lists, TextWriter output)
        {
            var labelWidth = stats.Max(kvp => kvp.Key.Length);
            var valueWidth = stats.Max(kvp => kvp.Value.ToString().Length);
            var statFormat = string.Format("[tr][td][b] {{0,-{0}}} [/b][/td][td] {{1,{1}}} [/td][/tr]", labelWidth, valueWidth);

            output.WriteLine("[table]");
            foreach (var kvp in stats)
            {
                output.WriteLine(statFormat, kvp.Key, kvp.Value);
            }
            output.WriteLine("[/table]");

            if (!_showList)
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
