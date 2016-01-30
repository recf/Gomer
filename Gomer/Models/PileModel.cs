using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Gomer.Models
{
    public class PileModel : BindableBase
    {
        public ObservableCollection<ListModel> Lists { get; private set; }
        public ObservableCollection<PlatformModel> Platforms { get; private set; }
        public ObservableCollection<StatusModel> Statuses { get; private set; }

        public ObservableCollection<GameModel> Games { get; private set; }

        public PileModel()
        {
            Lists = new ObservableCollection<ListModel>();
            Platforms = new ObservableCollection<PlatformModel>();
            Statuses = new ObservableCollection<StatusModel>();

            Games = new ObservableCollection<GameModel>();
        }
    }
}
