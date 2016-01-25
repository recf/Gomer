using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Models
{
    public class PlatformModel : ModelBase<PlatformModel>
    {
        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        public override void SetFrom(PlatformModel other)
        {
            Id = other.Id;
            Name = other.Name;
        }
    }
}
