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
            var commands = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof (Program));

            var statusCode = 0;
            try
            {
                statusCode = ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
            }
            catch (ArgumentException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Exception: {0}", e.Message);
                Console.ResetColor();
                statusCode = 1;
            }
            catch (CommandException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Exception: {0}", e.Message);
                Console.ResetColor();
                statusCode = e.StatusCode;
            }

            Environment.Exit(statusCode);
        }
    }
}
