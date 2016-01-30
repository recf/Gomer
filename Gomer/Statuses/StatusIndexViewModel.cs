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
    public class StatusIndexViewModel : IndexViewModelBase<StatusModel, StatusListViewModel, StatusDetailViewModel>
    {
        public StatusIndexViewModel(ObservableCollection<StatusModel> models) : 
            base(models, new StatusListViewModel(models), new StatusDetailViewModel())
        {
        }
    }
}
