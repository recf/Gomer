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

            HasAdditionalArguments(1, "<name>");
        }

        #region Overrides of ConsoleCommand

        public override int Run(string[] remainingArguments)
        {
            var pile = Helpers.ReadFile();
            if (pile == null)
            {
                return 1;
            }

            var name = remainingArguments[0];

            var game = pile.Games.FirstOrDefault(g => String.Equals(g.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (game == default(PileGame))
            {
                Console.WriteLine("No game with that name found.");
                return 1;
            }

            Console.WriteLine("Update game:");
            Console.WriteLine("============");

            // TODO: Format output with Helpers.ShowTable()
            var auditFormat = "{0,12}: {1} -> {2}";

            if (!string.IsNullOrWhiteSpace(_newName))
            {
                Console.WriteLine(auditFormat, "Name", game.Name, _newName);
                game.Name = _newName;
            }
            
            if (!string.IsNullOrWhiteSpace(_platform))
            {
                Console.WriteLine(auditFormat, "Platform", game.Platform, _platform);
                game.Platform = _platform;
            }

            if (_priority.HasValue)
            {
                Console.WriteLine(auditFormat, "Priority", game.Priority, _priority.Value);
                game.Priority = _priority.Value;
            }

            if (_hours.HasValue)
            {
                Console.WriteLine(auditFormat, "Est. Hours", game.EstimatedHours, _hours.Value);
                game.EstimatedHours = _hours.Value;
            }

            if (_addedDate.HasValue)
            {
                Console.WriteLine(auditFormat, "Added", Helpers.DateToString(game.AddedDate), Helpers.DateToString(_addedDate));
                game.AddedDate = _addedDate.Value;
            }

            if (_finishedDate.HasValue)
            {
                _playing = false;
                Console.WriteLine(auditFormat, "Finished", Helpers.DateToString(game.FinishedDate), Helpers.DateToString(_finishedDate));
                game.FinishedDate = _finishedDate.Value;
            }

            if (_playing.HasValue)
            {
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
                Console.WriteLine(auditFormat, "Genres", oldGenres, newGenres);
            }

            Console.WriteLine();
            Helpers.Show(game);

            Helpers.WriteFile(pile);

            return 0;
        }

        #endregion
    }
}
