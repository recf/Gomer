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
        private string _platform;

        private string _name;

        private int? _priority;

        private string _genre;

        public ShowCommand()
        {
            IsCommand("show", "Show games in pile, with optional filtering.");

            HasOption("n|name-like=", "Filter by part of the {NAME}.", v => _name = v);
            // TODO: Priority, platform and genre filters should probably be ONE-OF filters.
            HasOption("l|platform-eq=", "Filter by {PLATFORM}.", v => _platform = v);
            HasOption("p|priority=", "Filter by {PRIORITY}.", (int v) => _priority = v);
            HasOption("g|genre=", "Filter by {GENRE}.", v => _genre = v);
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

            if (!string.IsNullOrWhiteSpace(_platform))
            {
                criteria.Add(string.Format("platform = '{0}'", _platform));
                games = games.Where(g => string.Equals(g.Platform, _platform, StringComparison.InvariantCultureIgnoreCase));
            }

            if (_priority.HasValue)
            {
                criteria.Add(string.Format("priority = {0}", _priority));
                games = games.Where(g => g.Priority == _priority.Value);
            }

            if (!string.IsNullOrWhiteSpace(_genre))
            {
                criteria.Add(string.Format("genre ~= '{0}'", _genre));
                games = games.Where(g => (g.Genres ?? new string[0]).Any(x => x.IndexOf(_genre, StringComparison.CurrentCultureIgnoreCase) >= 0));
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
