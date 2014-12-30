using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;

namespace Gomer.Cli.Commands
{
    public class AboutCommand : ConsoleCommand
    {
        public AboutCommand()
        {
            IsCommand("about", "Show information about the app.");
        }

        #region Overrides of ConsoleCommand

        public override int Run(string[] remainingArguments)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var name = assembly.GetName().Name;

            var versionAttr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            var descAttr = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();

            Console.WriteLine("{0} v{1} - {2}", name, versionAttr.InformationalVersion, descAttr.Description);

            return 0;
        }

        #endregion
    }
}
