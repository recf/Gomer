using System.ComponentModel;
using Newtonsoft.Json;

namespace Gomer.DataAccess.Dto
{
    [JsonObject(MemberSerialization.OptIn)]
    public class StatusDto
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("order", Required = Required.Always)]
        public int Order { get; set; }
        
        [JsonProperty("always_in_stats", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(false)]
        public bool AlwaysIncludeInStats { get; set; }
    }
}
