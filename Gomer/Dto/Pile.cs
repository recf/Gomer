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
    public class Pile
    {
        [DataMember(Name="games", IsRequired = true)]
        public IList<PileGame> Games { get; set; }
    }
}
