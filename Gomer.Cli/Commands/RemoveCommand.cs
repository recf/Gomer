using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Core;
using ManyConsole;

namespace Gomer.Cli.Commands
{
    public class RemoveCommand : ConsoleCommand
    {
        public RemoveCommand()
        {
            IsCommand("rm", "Remove a pile game.");

            HasAdditionalArguments(1, " <name or alias>");
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

            pile.Games.Remove(game);

            Console.WriteLine("Removing game from pile.");
            Helpers.Show(game);

            Helpers.WriteFile(pile);

            return 0;
        }
    }
}
