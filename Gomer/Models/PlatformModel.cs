using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Models
{
    public class PlatformModel : ModelBase<PlatformModel>
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                Set(() => Name, ref _name, value);
            }
        }

        public override void SetFrom(PlatformModel other)
        {
            Id = other.Id;
            Name = other.Name;
        }
    }
}
