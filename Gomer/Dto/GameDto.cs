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
        [JsonProperty("id", Required = Required.Always)]
        public Guid Id { get; set; }

        [JsonProperty("list", Required = Required.Always)]
        public string List { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("platform", Required = Required.Always)]
        public string Platform { get; set; }

        [JsonProperty("added_on")]
        public DateTime AddedOn { get; set; }

        [JsonProperty("started_on", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public DateTime? StartedOn { get; set; }

        [JsonProperty("finished_on", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public DateTime? FinishedOn { get; set; }

        [JsonProperty("esimated_hours", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public decimal? EstimatedHours { get; set; }

        [JsonProperty("played_hours", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public decimal? PlayedHours { get; set; }
    }
}