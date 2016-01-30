using System.Collections.ObjectModel;
using Gomer.DataAccess;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Lists
{
    public class ListListViewModel : ListViewModelBase<IListRepository, ListModel>
    {
        public ListListViewModel(IListRepository repository) : base(repository)
        {
        }
    }
}
