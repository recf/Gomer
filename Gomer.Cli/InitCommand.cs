using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Core;
using ManyConsole;

namespace Gomer.Cli
{
    public class InitCommand : ConsoleCommand
    {
        public InitCommand()
        {
            _name = Environment.UserName;

            IsCommand("init", "Create a new .pile file.");

            HasOption("n|name=", string.Format("Base {{NAME}} of the file. (default: {0})", _name), v=> _name = v);
            HasOption("i|import-csv=", "CSV {FILE} to import.", v => _importCsvFile = v);
        }

        private string _name;

        private string _importCsvFile;

        #region Overrides of ConsoleCommand

        public override int Run(string[] remainingArguments)
        {
            var fs = Helpers.GetCandidatesFiles();

            var msg = @"Cannot initialize because the following *.pile file already exists in the 
current directory:

{0}

If you want to start over, please use a different directory, or delete this file
first.";

            if (fs.Any())
            {
                Console.WriteLine(msg, Path.GetFileName(fs.First()));
                return 1;
            }

            Pile pile = null;

            if (string.IsNullOrWhiteSpace(_importCsvFile))
            {
                pile = new Pile();
            }
            else
            {
                pile = Helpers.ReadCsvFile(_importCsvFile);
            }

            var fileName = _name + ".pile";

            Helpers.WriteFile(pile, fileName);

            Console.WriteLine("Created {0}", fileName);
            return 0;
        }

        #endregion
    }
}
