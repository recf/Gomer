using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gomer.Cli.Exceptions;
using Gomer.Core;
using ManyConsole;

namespace Gomer.Cli.Commands
{
    public class AddCommand : ConsoleCommand
    {
        private IList<string> _genres;

        private int _hours;

        private int _priority;

        private DateTime _addedDate;

        private string _alias;

        public AddCommand()
        {
            IsCommand("add", "Add a pile game.");

            _addedDate = DateTime.Today;
            _priority = 2;
            _hours = 10;
            _genres = new List<string>();
            
            HasOption("a|added-date=", string.Format("{{DATE}} acquired. (default: {0:yyyy-MM-dd})", _addedDate), (DateTime v) => _addedDate = v);
            HasOption("p|priority=", string.Format("{{PRIORITY}} of the game. (default: {0})", _priority), (int v) => _priority = v);
            HasOption("h|hours=", string.Format("Estimated {{HOURS}} to complete. (default: {0})", _hours), (int v) => _hours = v);
            HasOption("g|genre=", "{GENRE} that the game belongs to. Can be specified multiple times.", v => _genres.Add(v));
            HasOption("alias=", "Alternate {ALIAS} for the game.", v => _alias = v);

            HasAdditionalArguments(2, "<name> <platform>");
        }

        public override int Run(string[] remainingArguments)
        {
            var pile= Helpers.ReadFile();
            if (pile == null)
            {
                return 1;
            }

            var name = remainingArguments[0];
            var platform = remainingArguments[1];

            if (pile.Games.Any(g => String.Equals(g.Name, name, StringComparison.CurrentCultureIgnoreCase)))
            {
                Console.WriteLine("A game with that name already exists.");
                return 1;
            }

            if (pile.Games.Any(g => String.Equals(g.Alias, _alias, StringComparison.CurrentCultureIgnoreCase)))
            {
                Console.WriteLine("A game with that alias already exists.");
                return 1;
            }

            var game = new PileGame
            {
                Name = name,
                Alias = _alias,
                Platform = platform,
                EstimatedHours = _hours,
                Priority = _priority,
                AddedDate = _addedDate,
                Genres = _genres
            };

            pile.Games.Add(game);

            Helpers.Show(game);

            Helpers.WriteFile(pile);

            return 0;
        }
    }
}
