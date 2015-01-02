using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Core;
using ManyConsole;

namespace Gomer.Cli.Commands
{
    public class EditCommand : ConsoleCommand
    {
        private string _newName;

        private DateTime? _addedDate;

        private DateTime? _finishedDate;

        private int? _priority;

        private int? _hours;

        private List<string> _genres;

        private bool _clearGenres = false;

        private string _platform;

        private bool? _playing;

        public EditCommand()
        {
            IsCommand("edit", "Edit a pile game.");

            _genres = new List<string>();

            HasOption("rename=", "Set the {NAME}.", v => _newName = v);
            HasOption("l|platform=", "Set the {PLATFORM}.", v => _platform = v);
            HasOption("a|added-date=", "Set the {DATE} acquired.", (DateTime v) => _addedDate = v);
            HasOption("f|finished-date=", "Set the {DATE} finished. Implies --not-playing.", (DateTime v) => _finishedDate = v);
            HasOption("finished", "Equivalent to --finished-date with today's date.", _ => _finishedDate = DateTime.Today);
            HasOption("p|priority=", "Set the {PRIORITY} of the game.", (int v) => _priority = v);
            HasOption("h|hours=", "Set the estimated {HOURS} to complete.", (int v) => _hours = v);
            HasOption("playing", "Set Playing to true.", _ => _playing = true);
            HasOption("not-playing", "Set Playing to false.", _ => _playing = false);
            HasOption(
                "clear-genres", 
                "Clear existing genre list. This is run before adding new genres.",
                v => _clearGenres = v != null);
            HasOption("g|genre=", "Add a {GENRE}. Can be specified multiple times.", v => _genres.Add(v));

            HasAdditionalArguments(1, "<name or alias>");
        }

        public override int Run(string[] remainingArguments)
        {
            var pile = Helpers.ReadFile();
            if (pile == null)
            {
                return 1;
            }

            var nameOrAlias = remainingArguments[0];

            var game = pile.Games.FirstOrDefault(g => 
                String.Equals(g.Name, nameOrAlias, StringComparison.CurrentCultureIgnoreCase) 
                || String.Equals(g.Alias, nameOrAlias, StringComparison.CurrentCultureIgnoreCase));
            if (game == default(PileGame))
            {
                Console.WriteLine("No game with that name found.");
                return 1;
            }

            Console.WriteLine("Update game:");
            Console.WriteLine("============");

            var audit = new List<Tuple<string, string, string>>();
            var tableDef = new Dictionary<string, Func<Tuple<string, string, string>, string>>()
            {
                { "Property", t => t.Item1 },
                { "Old Value", t => t.Item2 },
                { "New Value", t => t.Item3 },
            };

            if (!string.IsNullOrWhiteSpace(_newName))
            {
                audit.Add(new Tuple<string, string, string>("Name", game.Name, _newName));
                game.Name = _newName;
            }
            
            if (!string.IsNullOrWhiteSpace(_platform))
            {
                audit.Add(new Tuple<string, string, string>("Platform", game.Platform, _platform));
                game.Platform = _platform;
            }

            if (_priority.HasValue)
            {
                audit.Add(new Tuple<string, string, string>("Priority", game.Priority.ToString(), _priority.Value.ToString()));
                game.Priority = _priority.Value;
            }

            if (_hours.HasValue)
            {
                audit.Add(new Tuple<string, string, string>("Est. Hours", game.EstimatedHours.ToString(), _hours.Value.ToString()));
                game.EstimatedHours = _hours.Value;
            }

            if (_addedDate.HasValue)
            {
                audit.Add(new Tuple<string, string, string>("Added", Helpers.DateToString(game.AddedDate), Helpers.DateToString(_addedDate)));
                game.AddedDate = _addedDate.Value;
            }

            if (_finishedDate.HasValue)
            {
                _playing = false;
                audit.Add(new Tuple<string, string, string>("Finished", Helpers.DateToString(game.FinishedDate), Helpers.DateToString(_finishedDate)));
                game.FinishedDate = _finishedDate.Value;
            }

            if (_playing.HasValue)
            {
                audit.Add(new Tuple<string, string, string>("Playing", game.Playing.ToString(), _playing.Value.ToString()));
                game.Playing = _playing.Value;
            }

            var updatingGenres = false;
            var oldGenres = string.Format("[{0}]", string.Join(", ", game.Genres ?? new string[0]));
            if (_clearGenres)
            {
                updatingGenres = true;
                if (game.Genres == null)
                {
                    game.Genres = new List<string>();
                }
                game.Genres.Clear();
            }

            if (_genres.Any())
            {
                updatingGenres = true;
                if (game.Genres == null)
                {
                    game.Genres = new List<string>();
                }
                _genres.ForEach(game.Genres.Add);
            }

            if (updatingGenres)
            {
                var newGenres = string.Format("[{0}]", string.Join(", ", game.Genres));
                audit.Add(new Tuple<string, string, string>("Genres", oldGenres, newGenres));
            }
            
            Helpers.ShowTable(tableDef, audit);

            Console.WriteLine();
            Helpers.Show(game);

            Helpers.WriteFile(pile);

            return 0;
        }
    }
}
