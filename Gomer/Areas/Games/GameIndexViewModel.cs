using System.Collections.ObjectModel;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Games
{
    public class GameIndexViewModel : IndexViewModelBase<GameModel, GameListViewModel, GameDetailViewModel>
    {
        public GameIndexViewModel(
            ObservableCollection<GameModel> models,
            ObservableCollection<ListModel> lists,
            ObservableCollection<PlatformModel> platforms,
            ObservableCollection<StatusModel> statuses)
            : base(models, 
            new GameListViewModel(models, lists), 
            new GameDetailViewModel(lists, platforms, statuses))
        {
        }
    }
}
