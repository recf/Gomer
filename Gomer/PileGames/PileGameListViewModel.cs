using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Gomer.Models;
using Recfab.Infrastructure;

namespace Gomer.PileGames
{
    public class PileGameListViewModel : ViewModelBase
    {
        public ObservableCollection<GameModel> Games { get; private set; }

        public PileGameListViewModel(ObservableCollection<GameModel> games)
        {
            Games = games;
        }
    }
}
