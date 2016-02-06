using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gomer.DataAccess.Dto
{
    [JsonObject(MemberSerialization.OptIn)]
    public class StatusHistoryDto
    {
        [JsonProperty("status_name", Required = Required.Always)]
        public string StatusName { get; set; }

        [JsonProperty("date", Required = Required.Always)]
        public DateTime StatusDate { get; set; }
    }
}
