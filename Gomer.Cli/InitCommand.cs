using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;

namespace Gomer.Cli
{
    public class InitCommand : ConsoleCommand
    {
        public InitCommand()
        {
            Name = Environment.UserName;

            IsCommand("init", "Create a new .pile file.");

            HasOption("n|name=", string.Format("Base {{NAME}} of the file. (default: {0})", Name), v=> Name = v);
            HasOption("i|import-csv=", "CSV {FILE} to import.", v => ImportCsvFile = v);
        }

        public string Name { get; set; }

        public string ImportCsvFile { get; set; }

        #region Overrides of ConsoleCommand

        public override int Run(string[] remainingArguments)
        {
            return 1;
            //throw new NotImplementedException();
        }

        #endregion
    }
}
