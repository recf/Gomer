using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gomer.Dto
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PileDto
    {
        [JsonProperty("games", Required = Required.Always)]
        public ICollection<GameDto> Games { get; set; }

        public PileDto()
        {
            Games = new List<GameDto>();
        }
    }
}
