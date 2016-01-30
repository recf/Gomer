using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Models
{
    public class StatusModel : ModelBase<StatusModel>
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

        private int _order;
        public int Order
        {
            get { return _order; }
            set
            {
                SetProperty(ref _order, value);
            }
        }

        private bool _alwaysIncludeInStats;
        public bool AlwaysIncludeInStats
        {
            get { return _alwaysIncludeInStats; }
            set { SetProperty(ref _alwaysIncludeInStats, value); }
        }

        public override void SetFrom(StatusModel other)
        {
            Id = other.Id;
            Name = other.Name;
            Order = other.Order;
            AlwaysIncludeInStats = other.AlwaysIncludeInStats;
        }
    }
}
