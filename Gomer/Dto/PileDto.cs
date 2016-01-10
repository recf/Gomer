using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gomer.Dto
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PileDto
    {
        [JsonProperty("pile", Required=Required.Always)]
        public IList<GameDto> PileGames { get; set; }

        [JsonProperty("wishlist", Required = Required.Always)]
        public IList<GameDto> WishlistGames { get; set; }

        [JsonProperty("ignored", Required = Required.Always)]
        public IList<GameDto> IgnoredGames { get; set; }
    }
}
