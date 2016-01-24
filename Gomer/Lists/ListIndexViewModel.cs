using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Gomer.Generic;
using Gomer.Models;
using Gomer.ViewModel;

namespace Gomer.Lists
{
    public class ListIndexViewModel : IndexViewModelBase<ListModel, ListListViewModel, ListDetailViewModel>
    {
        public ListIndexViewModel(ObservableCollection<ListModel> models)
            : base(models, new ListListViewModel(models), new ListDetailViewModel())
        {
            
        }
    }
}
