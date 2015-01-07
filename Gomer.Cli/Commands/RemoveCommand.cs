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

            HasAdditionalArguments(1, " <name or alias>");
        }

        public override void Run(string[] remainingArguments, TextWriter output)
        {
            var pile = ReadFile();

            var nameOrAlias = remainingArguments[0];

            var game = pile.Games.FirstOrDefault(g => 
                String.Equals(g.Name, nameOrAlias, StringComparison.CurrentCultureIgnoreCase) 
                || String.Equals(g.Alias, nameOrAlias, StringComparison.CurrentCultureIgnoreCase));
            if (game == default(PileGame))
            {
                throw new CommandException("No game with that name found.");
            }

            pile.Games.Remove(game);

            output.WriteLine("Removing game from pile.");
            Show(game, output);

            WriteFile(pile, output);
        }
    }
}
