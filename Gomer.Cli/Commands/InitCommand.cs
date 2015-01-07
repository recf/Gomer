using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Gomer.Core;
using ManyConsole;

namespace Gomer.Cli.Commands
{
    public class InitCommand : BaseCommand
    {
        private string _name;

        public InitCommand()
        {
            _name = Environment.UserName;

            IsCommand("init", "Create a new .pile file.");

            HasOption("n|name=", String.Format("Base {{NAME}} of the file. (default: {0})", _name), v=> _name = v);
        }

        public override void Run(string[] remainingArguments, TextWriter output)
        {
            var fs = GetCandidatesFiles();

            var msg = @"Cannot initialize because the following *.pile file already exists in the 
current directory:

{0}

If you want to start over, please use a different directory, or delete this file
first.";

            if (fs.Any())
            {
                throw new CommandException(string.Format(msg, Path.GetFileName(fs.First())));
            }

            var pile = new Pile();

            var fileName = _name + ".pile";

            WriteFile(pile, fileName, output);
        }
    }
}
