using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;


using Gomer.Events;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Games
{
    public class GameDetailViewModel : DetailViewModelBase<GameModel>
    {
        public ObservableCollection<ListModel> Lists { get; private set; }
        public ObservableCollection<PlatformModel> Platforms { get; private set; }
        
        public GameDetailViewModel(
            ObservableCollection<ListModel> lists, 
            ObservableCollection<PlatformModel> platforms)
        {
            Lists = lists;
            Platforms = platforms;
        }
    }
}
