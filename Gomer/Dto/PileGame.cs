using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Gomer.Dto
{
    [DataContract]
    public class PileGame
    {
        [DataMember(Name="name", IsRequired = true)]
        public string  Name { get; set; }

        [DataMember(Name = "platform_key", IsRequired = true)]
        public string PlatformKey { get; set; }
    }
}