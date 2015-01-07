using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;

namespace Gomer.Cli.Commands
{
    public class AboutCommand : BaseCommand
    {
        public AboutCommand()
        {
            IsCommand("about", "Show information about the app.");
        }

        public override void Run(string[] remainingArguments, TextWriter output)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var name = assembly.GetName().Name;

            var versionAttr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            var descAttr = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();

            output.WriteLine("{0} v{1} - {2}", name, versionAttr.InformationalVersion, descAttr.Description);
        }
    }
}
