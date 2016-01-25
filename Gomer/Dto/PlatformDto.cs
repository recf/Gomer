using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gomer.Dto
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PlatformDto
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
    }
}
