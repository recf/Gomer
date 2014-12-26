﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Gomer.Core
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PileGame
    {
        [JsonProperty("name", Required = Required.Always)]
        public string  Name { get; set; }

        [JsonProperty("platform", Required = Required.Always)]
        public string Platform { get; set; }

        [JsonProperty("on_pile_date", Required = Required.Always)]
        public DateTime OnPileDate { get; set; }

        [JsonProperty("priority", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(2)]
        public int Priority { get; set; }

        [JsonProperty("hours", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(10)]
        public int EstimatedHours { get; set; }

        [JsonProperty("genres", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<string> Genres { get; set; }
    }
}