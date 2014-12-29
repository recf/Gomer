using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Gomer.Core;
using ManyConsole;

namespace Gomer.Cli.Commands
{
    public class InitCommand : ConsoleCommand
    {
        private string _name;

        public InitCommand()
        {
            _name = Environment.UserName;

            IsCommand("init", "Create a new .pile file.");

            HasOption("n|name=", String.Format("Base {{NAME}} of the file. (default: {0})", _name), v=> _name = v);
        }

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

            var pile = new Pile();

            var fileName = _name + ".pile";

            Helpers.WriteFile(pile, fileName);
            return 0;
        }

        #endregion
    }
}
