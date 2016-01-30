using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Games
{
    public class GameListViewModel : ListViewModelBase<GameModel>
    {
        private ListModel _filterList;
        public ListModel FilterList
        {
            get { return _filterList; }
            set
            {
                SetProperty(ref _filterList, value);
                ApplyFilter();
            }
        }
        
        private ICollection<ListModel> _lists;
        public ICollection<ListModel> Lists
        {
            get { return _lists; }
            set
            {
                SetProperty(ref _lists, value);
            }
        }

        public GameListViewModel(ObservableCollection<GameModel> models, ICollection<ListModel> lists)
            : base(models)
        {
            Lists = lists;
            FilterList = lists.FirstOrDefault();
        }

        public GameListViewModel() : this(new ObservableCollection<GameModel>(), new List<ListModel>())
        {
        }

        public override bool Filter(GameModel model)
        {
            return model.List == FilterList; 
        }
    }
}
