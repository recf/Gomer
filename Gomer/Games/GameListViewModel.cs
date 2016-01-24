using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gomer.Events;
using Gomer.Generic;
using Gomer.Models;
using Gomer.ViewModel;

namespace Gomer.Games
{
    public class GameListViewModel : ListViewModelBase<GameModel>
    {
        private ListModel _filterList;
        public ListModel FilterList
        {
            get { return _filterList; }
            set
            {
                Set(() => FilterList, ref _filterList, value);
            }
        }
        
        private ICollection<ListModel> _lists;
        public ICollection<ListModel> Lists
        {
            get { return _lists; }
            set
            {
                Set(() => Lists, ref _lists, value);
            }
        }

        public GameListViewModel(ObservableCollection<GameModel> models, ICollection<ListModel> lists)
            : base(models)
        {
            Lists = lists;
            FilterList = lists.FirstOrDefault();
        }

        public bool Filter(object item)
        {
            var game = (GameModel)item;

            return game.List == FilterList; 
        }
    }
}
