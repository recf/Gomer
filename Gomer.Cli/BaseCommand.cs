using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Gomer.Core;
using ManyConsole;

namespace Gomer.Cli
{
    public abstract class BaseCommand : ConsoleCommand
    {
        public abstract void Run(string[] remainingArguments, TextWriter output);

        private string _outFile;

        protected BaseCommand()
        {
            _outFile = "-";

            HasOption("o|outfile=", string.Format("{{FILE}} to write output to. Use - for stdout. (default: {0})", _outFile),
                v => _outFile = v);
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                if (_outFile == "-")
                {
                    Run(remainingArguments, Console.Out);
                }
                else
                {
                    using (TextWriter output = new StreamWriter(_outFile, false, new UTF8Encoding(false)))
                    {
                        Run(remainingArguments, output);
                    }
                }
            }
            catch (CommandException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Exception: {0}", e.Message);
                Console.ResetColor();
                return e.StatusCode;
            }

            return 0;
        }

        public static IList<string> GetCandidatesFiles()
        {
            return Directory.GetFiles(Directory.GetCurrentDirectory(), "*.pile");
        }

        public static string ChooseFile()
        {
            var candidates = GetCandidatesFiles();

            if (candidates.Count == 0)
            {
                throw new CommandException("No .pile file found. Create one with the `init` command.");
            }

            if (candidates.Count > 1)
            {
                var message = new StringBuilder();
                message.AppendLine("Multiple .pile files found. Please move or delete one of them.");
                message.AppendLine();
                
                foreach (var fileName in candidates)
                {
                    message.AppendLine("* " + Path.GetFileName(fileName));
                }

                throw new CommandException(message.ToString());
            }

            return candidates.First();
        }

        public static Pile ReadFile(TextWriter output)
        {
            return ReadFile(ChooseFile(), output);
        }

        public static Pile ReadFile(string fileName, TextWriter output)
        {
            output.WriteLine("Reading {0}", Path.GetFileName(fileName));
            output.WriteLine();

            return PileManager.DeserializeFile(fileName);
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

        public static void Show(PileGame game)
        {
            Show(new[] { game });
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
                { "Alias", g => g.Alias ?? string.Empty },
                { "Platform", g => g.Platform },
                { "Pri.", g => g.Priority.ToString() },
                { "Hrs.", g => g.EstimatedHours.ToString() },
                { "Added", g => DateToString(g.AddedDate) },
                { "Finished", g => DateToString(g.FinishedDate) },
                { "Playing", g => g.Playing ? "yes" : "" },
                { "Genres", g => string.Join(", ", g.Genres ?? new string[0]) },
            };

            var items = games.OrderBy(g => g.Name).ToList();

            ShowTable(tableDef, items);
        }

        public static void ShowTable<T>(Dictionary<string, Func<T, string>> tableDef, IList<T> items)
        {
            if (!items.Any())
            {
                return;
            }

            var indexes = Enumerable.Range(0, tableDef.Count).ToArray();

            var headers = tableDef.Select(kvp => kvp.Key).ToArray();

            var tableData = items.Select(g => tableDef.Select(kvp => kvp.Value(g)).ToArray()).ToArray();

            var lengths = indexes.Select(i => tableData.Select(r => Math.Max(headers[i].Length, (r[i] ?? string.Empty).Length)).Max()).ToArray();

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
