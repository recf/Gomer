using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gomer.Core
{
    public static class PileManager
    {
        public static Pile DeserializeFile(string fileName)
        {
            var contents = File.ReadAllText(fileName);
            return Deserialize(contents);
        }

        public static Pile Deserialize(string contents)
        {
            return JsonConvert.DeserializeObject<Pile>(contents);
        }

        public static void SerializeToFile(Pile pile, string fileName)
        {
            var contents = Serialize(pile);
            File.WriteAllText(fileName, contents);
        }

        public static string Serialize(Pile pile)
        {
            var dateConverter = new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd" };

            return JsonConvert.SerializeObject(pile, Formatting.Indented, dateConverter);
        }
    }
}
