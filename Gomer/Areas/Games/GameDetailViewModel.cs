using System.Collections.ObjectModel;
using Gomer.DataAccess;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Games
{
    public class GameDetailViewModel : DetailViewModelBase<GameModel>
    {
        private readonly IListRepository _listRepo;
        private readonly IPlatformRepository _platformRepo;
        private readonly IStatusRepository _statusRepo;

        public ObservableCollection<ListModel> Lists { get; private set; }
        public ObservableCollection<PlatformModel> Platforms { get; private set; }
        public ObservableCollection<StatusModel> Statuses { get; private set; }
        
        public GameDetailViewModel(
            IListRepository lists, 
            IPlatformRepository platforms,
            IStatusRepository statuses)
        {
            _listRepo = lists;
            _platformRepo = platforms;
            _statusRepo = statuses;

            Lists = new ObservableCollection<ListModel>();
            Platforms = new ObservableCollection<PlatformModel>();
            Statuses = new ObservableCollection<StatusModel>();
        }

        public override void RefreshLookupData()
        {
            base.RefreshLookupData();

            Lists.Clear();
            foreach (var model in _listRepo.GetAll())
            {
                Lists.Add(model);
            }

            Platforms.Clear();
            foreach (var model in _platformRepo.GetAll())
            {
                Platforms.Add(model);
            }

            Statuses.Clear();
            foreach (var model in _statusRepo.GetAll())
            {
                Statuses.Add(model);
            }
        }
    }
}
