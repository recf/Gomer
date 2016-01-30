using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gomer.DataAccess;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Games
{
    public class GameListViewModel : ListViewModelBase<IGameRepository, GameModel>
    {
        private readonly IListRepository _listRepository;

        private ListModel _filterList;
        public ListModel FilterList
        {
            get { return _filterList; }
            set
            {
                SetProperty(ref _filterList, value);
                RefreshData();
            }
        }

        public ObservableCollection<ListModel> Lists { get; private set; }

        public GameListViewModel(IGameRepository repository, IListRepository lists)
            : base(repository)
        {
            _listRepository = lists;

            Lists = new ObservableCollection<ListModel>();
        }

        public override bool Filter(GameModel model)
        {
            return model.List == FilterList; 
        }

        public override void RefreshLookupData()
        {
            base.RefreshLookupData();

            Lists.Clear();
            foreach (var model in _listRepository.GetAll())
            {
                Lists.Add(model);
            }

            if (FilterList == null)
            {
                FilterList = Lists.First();
            }
        }
    }
}
