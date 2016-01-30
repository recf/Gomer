using System.Collections.ObjectModel;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Lists
{
    public class ListIndexViewModel : IndexViewModelBase<ListModel, ListListViewModel, ListDetailViewModel>
    {
        public ListIndexViewModel(ObservableCollection<ListModel> models)
            : base(models, new ListListViewModel(models), new ListDetailViewModel())
        {
            
        }
    }
}
