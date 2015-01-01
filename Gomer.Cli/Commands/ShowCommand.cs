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
        private List<string> _platforms;

        private string _name;

        private List<int> _priorities;

        private string _genre;

        private bool? _playing;

        private bool? _finished;

        public ShowCommand()
        {
            _platforms = new List<string>();
            _priorities = new List<int>();

            IsCommand("show", "Show games in pile, with optional filtering.");

            HasOption("n|name-like=", "Filter by part of the {NAME}.", v => _name = v);
            // TODO: Priority and genre filters should probably be ONE-OF filters.
            HasOption("l|platform=", "Filter by {PLATFORM}. Can be specified multiple times. This is a ONE-OF filter.", v => _platforms.Add(v));
            HasOption("p|priority=", "Filter by {PRIORITY}. Can be specified multiple times. This is a ONE-OF filter.", (int v) => _priorities.Add(v));
            HasOption("g|genre=", "Filter by {GENRE}.", v => _genre = v);
            HasOption("playing", "Filter by Playing.", _ => _playing = true);
            HasOption("not-playing", "Filter by not Playing.", _ => _playing = false);
            HasOption("finished", "Filter by Finished", _ => _finished = true);
            HasOption("u|unfinished", "Filter by not Finished", _ => _finished = false);
        }

        #region Overrides of ConsoleCommand

        public override int Run(string[] remainingArguments)
        {
            var pile = Helpers.ReadFile();
            if (pile == null)
            {
                return 1;
            }

            IEnumerable<PileGame> games = pile.Games;
            var criteria = new List<string>();

            if (!string.IsNullOrWhiteSpace(_name))
            {
                criteria.Add(string.Format("name ~= '{0}'", _name));
                games = games.Where(g => g.Name.IndexOf(_name, StringComparison.CurrentCultureIgnoreCase) >= 0);
            }

            if (_platforms.Any())
            {
                criteria.Add(string.Format("platform = '{0}'", string.Join("' or '", _platforms)));
                games = games.Where(g => _platforms.Any(p => string.Equals(g.Platform, p, StringComparison.InvariantCultureIgnoreCase)));
            }

            if (_priorities.Any())
            {
                criteria.Add(string.Format("priority = {0}", _priorities));
                games = games.Where(g => _priorities.Any(p => g.Priority == p));
            }

            if (!string.IsNullOrWhiteSpace(_genre))
            {
                criteria.Add(string.Format("genre ~= '{0}'", _genre));
                games = games.Where(g => (g.Genres ?? new string[0]).Any(x => x.IndexOf(_genre, StringComparison.CurrentCultureIgnoreCase) >= 0));
            }

            if (_playing.HasValue)
            {
                criteria.Add(string.Format("playing = {0}", _playing.Value));
                games = games.Where(g => g.Playing == _playing.Value);
            }

            if (_finished.HasValue)
            {
                criteria.Add(string.Format("finished date {0} empty", _finished.Value ? "is not" : "is"));
                games = games.Where(g => g.FinishedDate.HasValue == _finished.Value);
            }

            if (criteria.Any())
            {
                Console.WriteLine("Criteria");
                Console.WriteLine("========");
                criteria.ForEach(Console.WriteLine);
                Console.WriteLine();
            }

            Helpers.Show(games.ToList());

            return 0;
        }

        #endregion
    }
}
