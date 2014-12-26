using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;

namespace Gomer.Cli
{
    public class AddCommand : ConsoleCommand
    {
        public AddCommand()
        {
            IsCommand("add", "Add a pile game.");

            OnPileDate = DateTime.Today;
            Priority = 2;
            Hours = 10;
            Genres = new List<string>();
            
            HasOption("d|on-pile-date=", string.Format("{{DATE}} acquired. (default: {0:yyyy-MM-dd})", OnPileDate), (DateTime v) => OnPileDate = v);
            HasOption("p|priority=", "{PRIORITY} of the game.", (int v) => Priority = v);
            HasOption("h|hours=", "Estimated {HOURS} to complete.", (int v) => Hours = v);
            HasOption("g|genre=", "{GENRE} that the game belongs to. Can be specified multiple times.", v => Genres.Add(v));

            HasAdditionalArguments(2, "<name> <platform>");
        }

        public IList<string> Genres { get; set; }

        public int Hours { get; set; }

        public int Priority { get; set; }
        
        public DateTime OnPileDate { get; set; }

        #region Overrides of ConsoleCommand

        public override int Run(string[] remainingArguments)
        {
            return 0;
        }

        #endregion
    }
}
