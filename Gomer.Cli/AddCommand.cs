using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;

namespace Gomer.Cli
{
    public class AddCommand : ConsoleCommand
    {
        public AddCommand()
        {
            IsCommand("add", "Add a pile game.");

            HasAdditionalArguments(2, "<name> <platform>");
        }

        #region Overrides of ConsoleCommand

        public override int Run(string[] remainingArguments)
        {
            return 0;
            throw new NotImplementedException();
        }

        #endregion
    }
}
