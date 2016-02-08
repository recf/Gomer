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
            }
        }

        public ObservableCollection<ListModel> Lists { get; private set; }

        public RelayCommand SearchCommand { get; private set; }

        public GameListViewModel(IGameRepository repository, IListRepository lists)
            : base(repository)
        {
            _listRepository = lists;

            Lists = new ObservableCollection<ListModel>();

            SearchCommand = new RelayCommand(RefreshData);
        }

        public override void RefreshData()
        {
            Models.Clear();

            var models = Repository.Find(x => x.List.Id == FilterList.Id);

            foreach (var model in models)
            {
                Models.Add(model);
            }
        }

        public override void RefreshLookupData()
        {
            // Filter is bound in the UI via ComboBox, so clearing the list, clears the value
            // Save filter
            var filterList = FilterList;

            Lists.Clear();
            foreach (var model in _listRepository.GetAll())
            {
                Lists.Add(model);
            }

            // Restore Filter
            FilterList = filterList ?? Lists.First();
        }
    }
}
