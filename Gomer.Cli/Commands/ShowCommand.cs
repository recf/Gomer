using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;

namespace Gomer.Cli.Commands
{
    public class ShowCommand : ConsoleCommand
    {
        public ShowCommand()
        {
            IsCommand("show", "Show games in pile, with optional filtering.");
        }

        #region Overrides of ConsoleCommand

        public override int Run(string[] remainingArguments)
        {
            var pile = Helpers.ReadFile();
            if (pile == null)
            {
                return 1;
            }

            Helpers.Show(pile.Games);

            return 0;
        }

        #endregion
    }
}
