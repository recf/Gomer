using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Cli.Exceptions;
using Gomer.Core;
using Newtonsoft.Json;

namespace Gomer.Cli
{
    public static class Helpers
    {
        public static IList<string> GetCandidatesFiles()
        {
            return Directory.GetFiles(Directory.GetCurrentDirectory(), "*.pile");
        }

        public static string ChooseFile()
        {
            var candidates = GetCandidatesFiles();

            if (candidates.Count == 0)
            {
                Console.WriteLine("No .pile file found. Create one with the `init` command.");
                return null;
            }

            if (candidates.Count > 1)
            {
                Console.WriteLine("Multiple .pile files found. Please move or delete one of them.");
                Console.WriteLine();
                foreach (var fileName in candidates)
                {
                    Console.WriteLine(Path.GetFileName(fileName));
                    return null;
                }
            }

            return candidates.First();
        }

        public static void WriteFile(Pile pile)
        {
            var fileName = ChooseFile();

            WriteFile(pile, fileName);
        }

        public static void WriteFile(Pile pile, string fileName)
        {
            var contents = JsonConvert.SerializeObject(pile, Formatting.Indented);

            File.WriteAllText(fileName, contents);
        }

        public static Pile ReadFile()
        {
            var fileName = ChooseFile();
            if (fileName == null)
            {
                return null;
            }

            return ReadFile(fileName);
        }

        public static Pile ReadFile(string fileName)
        {
            var contents = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<Pile>(contents);
        }

        public static Pile ReadCsvFile(string importCsvFile)
        {
            throw new NotImplementedException();
        }

        public static void Show(PileGame game)
        {
            Show(new[] { game });
        }

        public static string DateToString(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return DateToString(dateTime.Value);
            }

            return string.Empty;
        }

        public static string DateToString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static void Show(IList<PileGame> games)
        {
            if (!games.Any())
            {
                Console.WriteLine("No games found with those criteria.");
                return;
            }

            var tableDef = new Dictionary<string, Func<PileGame, string>>()
            {
                { "Game", g => g.Name },
                { "Platform", g => g.Platform },
                { "Pri.", g => g.Priority.ToString() },
                { "Hrs.", g => g.EstimatedHours.ToString() },
                { "Genres", g => string.Join(", ", g.Genres ?? new string[0]) },
                { "Added", g => DateToString(g.AddedDate) },
                { "Finished", g => DateToString(g.FinishDate) },
            };

            var indexes = Enumerable.Range(0, tableDef.Count).ToArray();

            var headers = tableDef.Select(kvp => kvp.Key).ToArray();

            var tableData = games.OrderBy(g => g.Name).Select(g => tableDef.Select(kvp => kvp.Value(g)).ToArray()).ToArray();

            var lengths = indexes.Select(i => tableData.Select(r => Math.Max(headers[i].Length, r[i].Length)).Max()).ToArray();

            var formatter = "{{0,-{0}}} ";

            foreach (var i in indexes)
            {
                Console.Write(string.Format(formatter, lengths[i]), headers[i]);                
            }
            Console.WriteLine();

            foreach (var len in lengths)
            {
                Console.Write(new String('=', len) + " ");
            }
            Console.WriteLine();

            foreach (var record in tableData)
            {
                foreach (var i in indexes)
                {
                    Console.Write(string.Format(formatter, lengths[i]), record[i]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
