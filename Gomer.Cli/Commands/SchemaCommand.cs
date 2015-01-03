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
    public class SchemaCommand : BaseCommand
    {
        public SchemaCommand()
        {
            IsCommand("schema", "Show the JSON Schema of a .pile file.");
        }

        public override void Run(string[] remainingArguments, TextWriter output)
        {
            var schema = PileManager.SerializeSchema();

            output.WriteLine(schema);
        }
    }
}
