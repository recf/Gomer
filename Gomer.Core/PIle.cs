using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gomer.Core
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Pile
    {
        public Pile()
        {
            Games = new List<PileGame>();
        }

        [JsonProperty("games", Required = Required.Always)]
        public IList<PileGame> Games { get; set; }
    }
}
