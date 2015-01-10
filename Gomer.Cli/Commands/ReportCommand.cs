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

        public ReportCommand()
        {
            var today = DateTime.Today;
            _beginDate = today.AddDays(-today.Day + 1);
            _endDate = _beginDate.AddMonths(1).AddDays(-1);

            IsCommand("report", "Show report of changes for a date range.");

            Arg(
                "begin", 
                "Begin {{DATE}} of the date range. (default: {0:})",
                v => _beginDate = v,
                'b',
                _beginDate);

            Arg(
                "end", 
                "End {{DATE}} of the date range. (default: {0:yyyy-MM-dd})", 
                v => _endDate = v,
                'e',
                _endDate);

            Flag("bbcode", "Output in BBCode format, for posting in forums.", _ => _useBBCode = true);
        }

        public override void Run(string[] remainingArguments, TextWriter output)
        {
            var pile = ReadFile();
            var addedGames = pile.Search(addedOnOrAfter: _beginDate, addedBeforeOrOn: _endDate).OrderBy(g => g.Name).ToList();
            var finished = pile.Search(finishedOnOrAfter: _beginDate, finishedBeforeOrOn: _endDate).OrderBy(g => g.Name).ToList();

            if (_useBBCode)
            {
                OutputBBCode(addedGames, finished, output);
            }
            else
            {
                OutputConsole(addedGames, finished, output);
            }
        }

        private void OutputConsole(IList<PileGame> addedGames, IList<PileGame> finished, TextWriter output)
        {
            var format = "{0} ({1})";
            output.WriteLine("Added");
            output.WriteLine("=====");
            output.WriteLine();
            foreach (var game in addedGames)
            {
                output.WriteLine(format, game.Name, game.Platform);
            }
            output.WriteLine();
            
            output.WriteLine("Finished");
            output.WriteLine("========");
            output.WriteLine();
            foreach (var game in finished)
            {
                output.WriteLine(format, game.Name, game.Platform);
            }
        }

        private void OutputBBCode(IList<PileGame> addedGames, IList<PileGame> finished, TextWriter output)
        {
            var format = "[*] {0} ({1})";
            output.WriteLine("[b]Added[/b]");
            output.WriteLine();
            output.WriteLine("[list]");
            foreach (var game in addedGames)
            {
                output.WriteLine(format, game.Name, game.Platform);
            }
            output.WriteLine("[/list]");
            output.WriteLine();

            output.WriteLine("[b]Finished[/b]");
            output.WriteLine();
            output.WriteLine("[list]");
            foreach (var game in finished)
            {
                output.WriteLine(format, game.Name, game.Platform);
            }
            output.WriteLine("[/list]");
            output.WriteLine();
        }
    }
}
