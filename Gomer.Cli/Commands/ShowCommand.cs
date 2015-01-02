using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Gomer.Core;
using ManyConsole;

namespace Gomer.Cli.Commands
{
    public class ShowCommand : ConsoleCommand
    {
        public enum OutputFormat
        {
            Console,
            Pile,
            Csv
        }

        private List<string> _platforms;

        private string _name;

        private List<int> _priorities;

        private List<string> _genres;

        private bool? _playing;

        private bool? _finished;

        private string _outFile;

        private OutputFormat _outFormat;

        public ShowCommand()
        {
            _platforms = new List<string>();
            _priorities = new List<int>();
            _genres = new List<string>();

            _outFile = "-";
            _outFormat = OutputFormat.Console;

            IsCommand("show", "Show games in pile, with optional filtering.");

            HasOption("n|name=", "Filter by part of the {NAME} or Alias.", v => _name = v);
            HasOption("l|platform=", "Filter by {PLATFORM}. Can be specified multiple times. This is a ONE-OF-EQUALS filter.", v => _platforms.Add(v));
            HasOption("p|priority=", "Filter by {PRIORITY}. Can be specified multiple times. This is a ONE-OF-EQUALS filter.", (int v) => _priorities.Add(v));
            HasOption("g|genre=", "Filter by {GENRE}. Can be specified multiple times. This is a ONE-OF-LIKE filter.", v => _genres.Add(v));

            HasOption("playing", "Filter by Playing.", _ => _playing = true);
            HasOption("not-playing", "Filter by not Playing.", _ => _playing = false);
            
            HasOption("finished", "Filter by Finished", _ => _finished = true);
            HasOption("u|unfinished", "Filter by not Finished", _ => _finished = false);

            HasOption("o|outfile=", string.Format("{{FILE}} to write output to. Use - for stdout. (default: {0})", _outFile),
                v => _outFile = v);
            HasOption("format=",
                string.Format(
                    "Output {{FORMAT}}. Must be one of [ {0} ]. (default: {1})", 
                    string.Join(", ", Enum.GetNames(typeof (OutputFormat))), 
                    _outFormat), 
                (OutputFormat v) => _outFormat = v);
        }
        
        public override int Run(string[] remainingArguments)
        {
            var pile = Helpers.ReadFile();
            if (pile == null)
            {
                return 1;
            }

            var games = pile.Search(_name, _platforms, _priorities, _genres, _playing, _finished);

            switch (_outFormat)
            {
                case OutputFormat.Csv:
                    OutputCsvFormat(games);
                    break;
                case OutputFormat.Pile:
                    OutputPileFormat(games);
                    break;
                case OutputFormat.Console:
                    OutputConsoleFormat(games);
                    break;
            }

            return 0;
        }

        private void OutputPileFormat(IList<PileGame> games)
        {
            var pile = new Pile { Games = games };

            if (_outFile == "-")
            {
                Console.WriteLine(PileManager.Serialize(pile));
            }
            else
            {
                PileManager.SerializeToFile(pile, _outFile);
            }
        }

        private void OutputCsvFormat(IList<PileGame> games)
        {
            throw new NotImplementedException();
        }

        private void OutputConsoleFormat(IList<PileGame> games)
        {
            var criteria = new List<string>();

            if (!string.IsNullOrWhiteSpace(_name))
            {
                criteria.Add(string.Format("name or alias ~= '{0}'", _name));
            }

            if (_platforms.Any())
            {
                criteria.Add(string.Format("platform = '{0}'", string.Join("' or '", _platforms)));
            }

            if (_priorities.Any())
            {
                criteria.Add(string.Format("priority = {0}", string.Join(" or ", _priorities)));
            }

            if (_genres.Any())
            {
                criteria.Add(string.Format("genre ~= '{0}'", string.Join("' or '", _genres)));
            }

            if (_playing.HasValue)
            {
                criteria.Add(string.Format("playing = {0}", _playing.Value));
            }

            if (_finished.HasValue)
            {
                criteria.Add(string.Format("finished date {0} empty", _finished.Value ? "is not" : "is"));
            }

            if (criteria.Any())
            {
                Console.WriteLine("Criteria");
                Console.WriteLine("========");
                criteria.ForEach(Console.WriteLine);
                Console.WriteLine();
            }

            Helpers.Show(games);
        }
    }
}
