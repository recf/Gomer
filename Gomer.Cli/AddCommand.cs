using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Cli.Exceptions;
using Gomer.Core;
using ManyConsole;

namespace Gomer.Cli
{
    public class AddCommand : ConsoleCommand
    {
        public AddCommand()
        {
            IsCommand("add", "Add a pile game.");

            _onPileDate = DateTime.Today;
            _priority = 2;
            _hours = 10;
            _genres = new List<string>();
            
            HasOption("d|on-pile-date=", string.Format("{{DATE}} acquired. (default: {0:yyyy-MM-dd})", _onPileDate), (DateTime v) => _onPileDate = v);
            HasOption("p|priority=", "{PRIORITY} of the game.", (int v) => _priority = v);
            HasOption("h|hours=", "Estimated {HOURS} to complete.", (int v) => _hours = v);
            HasOption("g|genre=", "{GENRE} that the game belongs to. Can be specified multiple times.", v => _genres.Add(v));

            HasAdditionalArguments(2, "<name> <platform>");
        }

        private IList<string> _genres;

        private int _hours;

        private int _priority;

        private DateTime _onPileDate;

        #region Overrides of ConsoleCommand

        public override int Run(string[] remainingArguments)
        {

            Pile pile;
            try
            {
                pile = Helpers.ReadFile();
            }
            catch (NoPileFileException)
            {
                Console.WriteLine("No .pile file found. Create one with the `init` command.");
                return 1;
            }
            catch (TooManyPileFilesException e)
            {
                Console.WriteLine("Multiple .pile files found. Please move or delete one of them.");
                Console.WriteLine();
                foreach (var fileName in e.Files)
                {
                    Console.WriteLine(Path.GetFileName(fileName));
                }
                return 1;
            }

            var name = remainingArguments[0];
            var platform = remainingArguments[1];

            if (pile.Games.Any(g => String.Equals(g.Name, name, StringComparison.CurrentCultureIgnoreCase)))
            {
                Console.WriteLine("A game with that name already exists.");
                return 1;
            }

            var game = new PileGame
            {
                Name = name,
                Platform = platform,
                EstimatedHours = _hours,
                Priority = _priority,
                OnPileDate = _onPileDate
            };

            pile.Games.Add(game);

            Helpers.WriteFile(pile);

            return 0;
        }

        #endregion
    }
}
