using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gomer.DataAccess.Dto
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PileDto
    {
        [JsonProperty("lists", Required = Required.Always)]
        public ICollection<ListDto> Lists { get; set; }

        [JsonProperty("platforms", Required = Required.Always)]
        public ICollection<PlatformDto> Platforms { get; set; }

        [JsonProperty("games", Required = Required.Always)]
        public ICollection<GameDto> Games { get; set; }

        public PileDto()
        {
            Lists = new List<ListDto>();
            Platforms = new List<PlatformDto>();

            Games = new List<GameDto>();
        }
    }
}
