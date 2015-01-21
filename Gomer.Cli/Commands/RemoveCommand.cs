using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Core;
using ManyConsole;

namespace Gomer.Cli.Commands
{
    public class RemoveCommand : BaseCommand
    {
        public RemoveCommand()
        {
            IsCommand("rm", "Remove a pile game.");

            HasAdditionalArguments(1, " <name>");
        }

        public override void Run(string[] remainingArguments, TextWriter output)
        {
            var pile = ReadFile();

            var name = remainingArguments[0];

            var games = pile.Search(name: name, hidden: null);

            if (!games.Any())
            {
                throw new CommandException("No game with that name found.");
            }

            if (games.Count > 1)
            {
                var msg = new StringBuilder();
                msg.AppendLine("Found multiple games. Please narrow your search.");
                foreach (var g in games)
                {
                    msg.AppendLine("* " + g.Name);
                }
                throw new CommandException(msg.ToString());
            }

            var game = games.First();

            pile.Games.Remove(game);

            output.WriteLine("Removing game from pile.");
            Show(game, output);

            WriteFile(pile, output);
        }
    }
}
