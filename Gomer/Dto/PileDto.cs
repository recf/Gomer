using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gomer.Dto
{
    [DataContract]
    public class PileDto
    {
        [DataMember(Name="games", IsRequired = true)]
        public IList<GameDto> Games { get; set; }
        
        [DataMember(Name = "wishlist", IsRequired = true)]
        public IList<GameDto> Wishlist { get; set; }

        [DataMember(Name = "ignored", IsRequired = true)]
        public IList<GameDto> IgnoreList { get; set; }
    }
}
