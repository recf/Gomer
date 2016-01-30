using System.Collections.ObjectModel;
using Gomer.DataAccess;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Games
{
    public class GameIndexViewModel : IndexViewModelBase<IGameRepository, GameModel, GameListViewModel, GameDetailViewModel>
    {
        public GameIndexViewModel(
            IGameRepository repository,
            IListRepository lists,
            IPlatformRepository platforms,
            IStatusRepository statuses)
            : base(repository,
            new GameListViewModel(repository, lists), 
            new GameDetailViewModel(lists, platforms, statuses))
        {
        }
    }
}
