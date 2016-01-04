using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Gomer.Dto
{
    [DataContract]
    public class GameDto
    {
        [DataMember(Name="name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "platform", IsRequired = true)]
        public string Platform { get; set; }
    }
}