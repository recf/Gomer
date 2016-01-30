using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Statuses
{
    public class StatusListViewModel : ListViewModelBase<StatusModel>
    {
        public StatusListViewModel(ObservableCollection<StatusModel> models) : base(models)
        {
        }
    }
}
