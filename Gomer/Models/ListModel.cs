using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Models
{
    public class ListModel : ModelBase<ListModel>
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

        private bool _includeInStats;
        public bool IncludeInStats
        {
            get { return _includeInStats; }
            set { SetProperty(ref _includeInStats, value); }
        }

        private int _gameCount;
        public int GameCount
        {
            get { return _gameCount; }
            set { SetProperty(ref _gameCount, value); }
        }

        public override void SetFrom(ListModel other)
        {
            Id = other.Id;
            Name = other.Name;
            IncludeInStats = other.IncludeInStats;

            GameCount = other.GameCount;
        }
    }
}
