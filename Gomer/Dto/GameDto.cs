using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Gomer.Dto
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GameDto
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("platform", Required = Required.Always)]
        public string Platform { get; set; }
    }
}