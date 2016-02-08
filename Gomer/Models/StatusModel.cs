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
        private StatusCodes _code;
        public StatusCodes Code
        {
            get { return _code; }
            set
            {
                SetProperty(ref _code, value);
                OnPropertyChanged("Name");
            }
        }
        
        public string Name
        {
            get { return Code.ToString(); }
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

        private int _gameCount;
        public int GameCount
        {
            get { return _gameCount; }
            set { SetProperty(ref _gameCount, value); }
        }

        public override void SetFrom(StatusModel other)
        {
            Id = other.Id;
            Code = other.Code;
            Order = other.Order;
            AlwaysIncludeInStats = other.AlwaysIncludeInStats;

            GameCount = other.GameCount;
        }
    }
}
