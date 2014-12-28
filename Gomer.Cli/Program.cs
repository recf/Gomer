using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;

namespace Gomer.Cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // TODO: Implement edit command
            var commands = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof (Program));

            ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }
    }
}
