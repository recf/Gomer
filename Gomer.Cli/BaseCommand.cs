using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Gomer.Core;
using ManyConsole;
using NDesk.Options;

namespace Gomer.Cli
{
    public abstract class BaseCommand : ConsoleCommand
    {
        public abstract void Run(string[] remainingArguments, TextWriter output);

        private string _outFile;

        private bool _verbose;

        private readonly List<KeyValuePair<string, object>> _trace;
        
        private bool _launchDebugger;

        protected BaseCommand()
        {
            SkipsCommandSummaryBeforeRunning();

            _trace = new List<KeyValuePair<string, object>>();

            _outFile = "-";
            _verbose = false;
            _launchDebugger = false;
        }

        public void Arg<T>(
            int position,
            string name,
            T defaultValue,
            string description,
            Func<string, T> read,
            Action<T> action,
            Func<T, string> show = null)
        {
            if (show == null)
            {
                show = v => Convert.ToString(v);
            }
        }

        public void Arg<T>(
            string longName,
            string description,
            Func<string, T> read,
            Action<T> action,
            char shortName = default(char),
            T defaultValue = default(T),
            Func<T, string> show = null)
        {
            if (show == null)
            {
                show = v => Convert.ToString(v);
            }

            var prototype = shortName == default(char) 
                ? string.Format("{0}=", longName) 
                : string.Format("{0}|{1}=", shortName, longName);

            var fullDesc = string.Format(description, show(defaultValue));

            Action<string> realAction = v =>
            {
                var value = read(v);

                _trace.Add(new KeyValuePair<string, object>(longName, show(value)));

                action(value);
            };

            HasOption(prototype, fullDesc, realAction);
        }

        /// <remarks>
        /// string arg
        /// </remarks>
        public void Arg(
            string longName,
            string description,
            Action<string> action,
            char shortName = default(char),
            string defaultValue = "")
        {
            Arg(
                longName,
                description,
                v => v == null ? null : v.Trim(),
                action,
                shortName,
                defaultValue);
        }

        /// <remarks>
        /// int arg
        /// </remarks>
        public void Arg(
            string longName,
            string description,
            Action<int> action,
            char shortName = default(char),
            int defaultValue = default(int))
        {
            Arg(
                longName,
                description,
                int.Parse,
                action,
                shortName,
                defaultValue);
        }
        
        /// <remarks>
        /// DateTime arg
        /// </remarks>
        public void Arg(
            string longName,
            string description,
            Action<DateTime> action,
            char shortName = default(char),
            DateTime defaultValue = default(DateTime))
        {
            Arg(
                longName,
                description,
                DateTime.Parse,
                action,
                shortName,
                defaultValue,
                v => v.ToString("yyyy-MM-dd"));
        }
        
        public void Flag(string longName, string description, Action<bool> action, char shortName = default(char))
        {
            var prototype = shortName == default(char) ? longName : string.Format("{0}|{1}", shortName, longName);
            Action<string> realAction = v =>
            {
                var flagSet = v != null;

                _trace.Add(new KeyValuePair<string, object>(longName, flagSet ? "+" : "-"));

                action(flagSet);
            };

            HasOption(prototype, description, realAction);
        }

        public void VerboseFlag(string description = "Increase verbosity of output.")
        {
            Flag("verbose", description, v => _verbose = v, 'v');
        }

        public void DebugFlag(string description = "Show values sent to options and launch debugger. Implies --trace.")
        {
            Flag("debug", description, v =>
            {
                _launchDebugger = v;
            });
        }

        public void OutfileArg(string description = "{{FILE}} to write output to. Use - for STDOUT. (default: {0})")
        {
            Arg("outfile", description, v => _outFile = v, 'o', _outFile);
        }

        public TEnum ReadEnum<TEnum>(string input)
        {
            return (TEnum) Enum.Parse(typeof (TEnum), input, true);
        }

        public void ShowTrace(TextWriter output)
        {
            foreach (var kvp in _trace)
            {
                output.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
            }
            output.WriteLine();
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                if (_outFile == "-")
                {
                    if (_launchDebugger)
                    {
                        ShowTrace(Console.Out);
                        Debugger.Launch();
                    }

                    Run(remainingArguments, Console.Out);
                }
                else
                {
                    using (TextWriter output = new StreamWriter(_outFile, false, new UTF8Encoding(false)))
                    {
                        if (_launchDebugger)
                        {
                            ShowTrace(output);
                            Debugger.Launch();
                        }

                        Run(remainingArguments, output);
                        Console.WriteLine("Writing output to {0}", _outFile);
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

        public void WriteLineMessage(string format, params object[] args)
        {
            if (format == null)
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(format, args);
            }
        }

        public void WriteLineVerbose(string format = null, params object[] args)
        {
            if (_verbose)
            {
                WriteLineMessage(format, args);
            }
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

        public Pile ReadFile()
        {
            return ReadFile(ChooseFile());
        }

        public Pile ReadFile(string fileName)
        {
            WriteLineVerbose("Reading {0}", Path.GetFileName(fileName));
            WriteLineVerbose();

            return PileManager.DeserializeFile(fileName);
        }

        public void WriteFile(Pile pile, TextWriter output)
        {
            var fileName = ChooseFile();

            WriteFile(pile, fileName, output);
        }

        public void WriteFile(Pile pile, string fileName, TextWriter output)
        {
            output.WriteLine("Writing {0}", Path.GetFileName(fileName));
            output.WriteLine();

            PileManager.SerializeToFile(pile, fileName);
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

        public static void Show(PileGame game, TextWriter output)
        {
            Show(new[] { game }, output);
        }

        public static void Show(IList<PileGame> games, TextWriter output)
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

            ShowTable(tableDef, games, output);
        }

        public static void ShowTable<T>(Dictionary<string, Func<T, string>> tableDef, IList<T> items, TextWriter output)
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
            output.WriteLine();

            foreach (var len in lengths)
            {
                output.Write(new String('=', len) + " ");
            }
            output.WriteLine();

            foreach (var record in tableData)
            {
                foreach (var i in indexes)
                {
                    output.Write(string.Format(formatter, lengths[i]), record[i]);
                }
                output.WriteLine();
            }
            output.WriteLine();
        }
    }
}
