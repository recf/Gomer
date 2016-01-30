using System;
using Newtonsoft.Json;

namespace Gomer.DataAccess.Dto
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GameDto
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("list_name", Required = Required.Always)]
        public string ListName { get; set; }

        [JsonProperty("platform_name", Required = Required.Always)]
        public string PlatformName { get; set; }

        [JsonProperty("status_name", Required = Required.Always)]
        public string StatusName { get; set; }

        [JsonProperty("added_on", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public DateTime? AddedOn { get; set; }

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