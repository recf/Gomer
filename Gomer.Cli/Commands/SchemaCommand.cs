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
    public class SchemaCommand : ConsoleCommand
    {
        private string _outFile;

        public SchemaCommand()
        {
            IsCommand("schema", "Show the JSON Schema of a .pile file.");

            _outFile = "-";

            HasOption("o|outfile=", string.Format("{{FILE}} to write output to. Use - for stdout. (default: {0})", _outFile),
                v => _outFile = v);
        }

        public override int Run(string[] remainingArguments)
        {
            var schema = PileManager.SerializeSchema();

            if (_outFile == "-")
            {
                Console.WriteLine(schema);
            }
            else
            {
                File.WriteAllText(_outFile, schema, new UTF8Encoding(false));
                Console.WriteLine("Writing schema to {0}", _outFile);
            }

            return 0;
        }
    }
}
