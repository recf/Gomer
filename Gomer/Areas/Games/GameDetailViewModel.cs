using System;
using System.Collections.ObjectModel;
using System.Linq;
using Gomer.DataAccess;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Games
{
    public class GameDetailViewModel : DetailViewModelBase<GameModel>
    {
        private readonly IListRepository _listRepo;
        private readonly IPlatformRepository _platformRepo;

        public ObservableCollection<ListModel> Lists { get; private set; }
        public ObservableCollection<PlatformModel> Platforms { get; private set; }

        private PlatformModel _platformSelectedForAdd;
        public PlatformModel PlatformSelectedForAdd
        {
            get { return _platformSelectedForAdd; }
            set { SetProperty(ref _platformSelectedForAdd, value);}
        }

        public RelayCommand<PlatformModel> AddPlatformCommand { get; private set; }
        public RelayCommand<PlatformModel> RemovePlatformCommand { get; private set; }

        public GameDetailViewModel(
            IListRepository lists, 
            IPlatformRepository platforms)
        {
            _listRepo = lists;
            _platformRepo = platforms;

            Lists = new ObservableCollection<ListModel>();
            Platforms = new ObservableCollection<PlatformModel>();

            AddPlatformCommand = new RelayCommand<PlatformModel>(x => Model.Platforms.Add(x));
            RemovePlatformCommand = new RelayCommand<PlatformModel>(x => Model.Platforms.Remove(x));
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
            PlatformSelectedForAdd = Platforms.First();
        }
    }
}
