using System.Collections.ObjectModel;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Statuses
{
    public class StatusListViewModel : ListViewModelBase<StatusModel>
    {
        public StatusListViewModel(ObservableCollection<StatusModel> models) : base(models)
        {
        }
    }
}
