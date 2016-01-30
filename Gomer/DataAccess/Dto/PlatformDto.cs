using Newtonsoft.Json;

namespace Gomer.DataAccess.Dto
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PlatformDto
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
    }
}
