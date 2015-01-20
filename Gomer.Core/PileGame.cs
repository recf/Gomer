using System;
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

        [JsonProperty("hidden", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(false)]
        public bool IsHidden { get; set; }

        [JsonProperty("platform", Required = Required.Always)]
        public string Platform { get; set; }

        [JsonProperty("added_date", Required = Required.Always)]
        public DateTime AddedDate { get; set; }

        [JsonProperty("finished_date", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public DateTime? FinishedDate { get; set; }

        [JsonProperty("hours", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(10)]
        public int EstimatedHours { get; set; }

        [JsonProperty("playing", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(false)]
        public bool Playing { get; set; }

        [JsonProperty("tags", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<string> Tags { get; set; }
    }
}