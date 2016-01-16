using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Gomer.Models
{
    public class PileModel : ObservableObject
    {
        public ObservableCollection<GameModel> Games { get; private set; }

        public PileModel()
        {
            Games = new ObservableCollection<GameModel>();
        }
    }
}
