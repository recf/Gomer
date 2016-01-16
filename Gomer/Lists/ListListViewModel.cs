using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Gomer.Models;

namespace Gomer.Lists
{
    public class ListListViewModel : ViewModelBase
    {
        public ObservableCollection<ListModel> Lists { get; private set; }

        public ListListViewModel(IEnumerable<ListModel> lists)
        {
            Lists = new ObservableCollection<ListModel>(lists);
        }

        #region Events

        public event EventHandler DataChanged;

        private void OnDataChanged()
        {
            if (DataChanged != null)
            {
                DataChanged(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
