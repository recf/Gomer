using System.Collections.ObjectModel;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Games
{
    public class GameDetailViewModel : DetailViewModelBase<GameModel>
    {
        public ObservableCollection<ListModel> Lists { get; private set; }
        public ObservableCollection<PlatformModel> Platforms { get; private set; }
        public ObservableCollection<StatusModel> Statuses { get; private set; }
        
        public GameDetailViewModel(
            ObservableCollection<ListModel> lists, 
            ObservableCollection<PlatformModel> platforms,
            ObservableCollection<StatusModel> statuses)
        {
            Lists = lists;
            Platforms = platforms;
            Statuses = statuses;
        }
    }
}
