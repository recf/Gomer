using System.ComponentModel;
using Newtonsoft.Json;

namespace Gomer.DataAccess.Dto
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ListDto
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("in_stats", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(true)]
        public bool IncludeInStats { get; set; }
    }
}
