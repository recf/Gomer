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
                throw new NoPileFileException();
            }

            if (candidates.Count > 1)
            {
                throw new TooManyPileFilesException(candidates);
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
    }
}
