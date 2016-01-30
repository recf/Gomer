using System.Collections.ObjectModel;
using Gomer.DataAccess;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Lists
{
    public class ListIndexViewModel : IndexViewModelBase<IListRepository, ListModel, ListListViewModel, ListDetailViewModel>
    {
        public ListIndexViewModel(IListRepository repository) :
            base(repository, 
            new ListListViewModel(repository), 
            new ListDetailViewModel())
        {
        }
    }
}
