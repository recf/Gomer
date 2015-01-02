using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Gomer.Core;
using ManyConsole;

namespace Gomer.Cli.Commands
{
    public class ImportCsvCommand : ConsoleCommand
    {
        private Dictionary<string, string> _fieldMap;

        public ImportCsvCommand()
        {
            _fieldMap = new Dictionary<string, string>
            {
                { "Name", "Name"},
                { "Platform", "Platform"},
                { "Priority", "Priority"},
                { "Hours", "Hours"},
                { "Genres", "Genres"},
                { "Added Date", "Added Date"},
                { "Finished Date", "Finished Date"},
            };

            IsCommand("import-csv", "Import a file in CSV format to an existing .pile file.");
            HasFieldMapOption("n|name-field=", "Name");
            HasFieldMapOption("l|platform-field=", "Platform");
            HasFieldMapOption("p|priority-field=", "Priority");
            HasFieldMapOption("h|hours-field=", "Hours");
            HasFieldMapOption("g|genres-field=", "Genres");
            HasFieldMapOption("a|added-date-field=", "Added Date");
            HasFieldMapOption("f|finished-date-field=", "Finished Date");

            HasAdditionalArguments(1, "<filename>");
        }

        private void HasFieldMapOption(string prototype, string key)
        {
            HasOption(
                prototype,
                string.Format("{{FIELD}} in CSV file to map to {0}. (default: '{1}')", key, _fieldMap[key]),
                v => _fieldMap[key] = v);
        }

        public override int Run(string[] remainingArguments)
        {
            var pile = Helpers.ReadFile();
            if (pile == null)
            {
                return 1;
            }

            var fileName = remainingArguments[0];
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Cannot find file {0}", fileName);
                return 1;
            }

            using (var textReader = File.OpenText(fileName))
            {
                var csv = new CsvReader(textReader);
                csv.Configuration.IsHeaderCaseSensitive = false;
                csv.Read();

                var headers = csv.FieldHeaders;

                var mapped = _fieldMap.Where(kvp => headers.Contains(kvp.Value, StringComparer.InvariantCultureIgnoreCase)).ToList();
                var unmapped = _fieldMap.Where(kvp => !headers.Contains(kvp.Value, StringComparer.InvariantCultureIgnoreCase)).ToList();
                
                var tableDef = new Dictionary<string, Func<KeyValuePair<string, string>, string>>
                {
                    { "Property", kvp => kvp.Key },
                    { "CSV Field", kvp => kvp.Value }
                };

                Console.WriteLine("Mapped the following fields in CSV file");
                Console.WriteLine();
                Helpers.ShowTable(tableDef, mapped);

                if (unmapped.Any())
                {
                    Console.WriteLine("Could not the following fields in CSV file");
                    Console.WriteLine();
                    Helpers.ShowTable(tableDef, unmapped);
                    return 1;
                }

                var updatedCount = 0;
                var addedCount = 0;
                while (csv.Read())
                {
                    var name = csv.GetField(_fieldMap["Name"]);

                    var game = pile.Games.FirstOrDefault(g => String.Equals(g.Name, name, StringComparison.CurrentCultureIgnoreCase));
                    if (game == default(PileGame))
                    {
                        game = new PileGame
                        {
                            Name = name
                        };
                        pile.Games.Add(game);
                        addedCount++;
                    }
                    else
                    {
                        updatedCount++;
                    }

                    game.Platform = csv.GetField(_fieldMap["Platform"]);

                    // TODO: Move default priority and default hours into constants, or read them from the attributes on the PileGame class.
                    game.Priority = GetFieldOrDefault(csv, "Priority", 2);
                    game.EstimatedHours = GetFieldOrDefault(csv, "Hours", 10);
                    game.Genres = csv.GetField(_fieldMap["Genres"]).Split(',');
                    game.AddedDate = csv.GetField<DateTime>(_fieldMap["Added Date"]);

                    game.FinishedDate = csv.GetField<DateTime?>(_fieldMap["Finished Date"]); ;
                }

                Console.WriteLine("Created {0} games, updated {1} existing games.", addedCount, updatedCount);
            }

            Helpers.WriteFile(pile);

            return 0;
        }

        private int GetFieldOrDefault(CsvReader csv, string key, int defaultValue)
        {
            var csvValue = csv.GetField(_fieldMap[key]);
            int value = 0;

            if (int.TryParse(csvValue, out value))
            {
                return value;
            }

            return defaultValue;
        }
    }
}
