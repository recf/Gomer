using System.Collections.ObjectModel;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Statuses
{
    public class StatusIndexViewModel : IndexViewModelBase<StatusModel, StatusListViewModel, StatusDetailViewModel>
    {
        public StatusIndexViewModel(ObservableCollection<StatusModel> models) : 
            base(models, new StatusListViewModel(models), new StatusDetailViewModel())
        {
        }
    }
}
