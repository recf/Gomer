using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Cli.Commands
{
    public class Config : BaseCommand
    {
        private bool? _trackHours;

        public Config()
        {
            IsCommand("config", "Configure pile tracking options.");

            Flag("track-hours", "Turn on hour tracking", _ => _trackHours = true);
            Flag("no-track-hours", "Turn off hour tracking", _ => _trackHours = false);
        }

        public override void Run(string[] remainingArguments, TextWriter output)
        {
            var pile = ReadFile();

            if (_trackHours.HasValue)
            {
                pile.ShouldTrackHours = _trackHours.Value;
            }

            WriteFile(pile, output);
        }
    }
}
