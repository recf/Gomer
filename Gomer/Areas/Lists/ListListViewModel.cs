using System.Collections.ObjectModel;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Lists
{
    public class ListListViewModel : ListViewModelBase<ListModel>
    {
        public ListListViewModel(ObservableCollection<ListModel> models) : base(models)
        {
            
        }
    }
}
