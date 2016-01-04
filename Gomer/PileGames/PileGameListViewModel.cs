using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Gomer.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Recfab.Infrastructure;

namespace Gomer.PileGames
{
    public class PileGameListViewModel : ViewModel
    {
        public ReactiveList<GameModel> Games { get; private set; }

        public PileGameListViewModel(ReactiveList<GameModel> games)
        {
            Games = games;
        }
    }
}
