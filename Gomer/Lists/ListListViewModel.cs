using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Gomer.Events;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Lists
{
    public class ListListViewModel : ListViewModelBase<ListModel>
    {
        public ListListViewModel(ObservableCollection<ListModel> models) : base(models)
        {
            
        }
    }
}
