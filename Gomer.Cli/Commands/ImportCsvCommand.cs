using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Gomer.Core;
using ManyConsole;

namespace Gomer.Cli.Commands
{
    public class ImportCsvCommand : BaseCommand
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
                { "Added Date", "Added Date"},
                { "Finished Date", "Finished Date"},
                { "Playing", "Playing" },
                { "Tags", "Tags"},
            };

            IsCommand("import-csv", "Import a file in CSV format to an existing .pile file.");
            HasFieldMapArg("name-field", "Name", 'n');
            HasFieldMapArg("platform-field", "Platform");
            HasFieldMapArg("priority-field", "Priority", 'p');
            HasFieldMapArg("hours-field", "Hours", 'H');
            HasFieldMapArg("genres-field", "Tags", 'g');
            HasFieldMapArg("added-date-field", "Added Date", 'a');
            HasFieldMapArg("finished-date-field", "Finished Date", 'f');

            HasAdditionalArguments(1, "<filename>");
        }

        private void HasFieldMapArg(string name, string key, char shortName = default(char))
        {
            Arg(
                name,
                string.Format("{{{{FIELD}}}} in CSV file to map to {0}. (default: {{0}})", key),
                v => _fieldMap[key] = v,
                shortName,
                _fieldMap[key]);
        }

        public override void Run(string[] remainingArguments, TextWriter output)
        {
            var pile = ReadFile();
            
            var fileName = remainingArguments[0];
            if (!File.Exists(fileName))
            {
                throw new CommandException(string.Format("Cannot find file {0}", fileName));
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

                output.WriteLine("Mapped the following fields in CSV file");
                output.WriteLine();
                ShowTable(tableDef, mapped, output);

                if (unmapped.Any())
                {
                    throw new CommandException("Could not map the following fields in CSV file:" + string.Join(", ", unmapped.Select(u => u.Key)));
                }

                var updatedCount = 0;
                var addedCount = 0;

                do
                {
                    var name = csv.GetField(_fieldMap["Name"]);
                    string alias;
                    csv.TryGetField(_fieldMap["Alias"], out alias);

                    var game = pile.Games.FirstOrDefault(g =>
                        String.Equals(g.Name, name, StringComparison.CurrentCultureIgnoreCase));
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
                    game.Tags = csv.GetField(_fieldMap["Tags"]).Split(',');
                    game.AddedDate = csv.GetField<DateTime>(_fieldMap["Added Date"]);
                    game.FinishedDate = csv.GetField<DateTime?>(_fieldMap["Finished Date"]);

                    var playing = "no";
                    csv.TryGetField(_fieldMap["Playing"], out playing);

                    game.Playing = new[] { "yes", "true", "1" }.Contains(playing,
                        StringComparer.InvariantCultureIgnoreCase);
                } while (csv.Read());

                output.WriteLine("Created {0} games, updated {1} existing games.", addedCount, updatedCount);
            }

            WriteFile(pile, output);
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
