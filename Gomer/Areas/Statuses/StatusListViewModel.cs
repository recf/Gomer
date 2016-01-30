using System.Collections.ObjectModel;
using Gomer.DataAccess;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Statuses
{
    public class StatusListViewModel : ListViewModelBase<IStatusRepository, StatusModel>
    {
        public StatusListViewModel(IStatusRepository repository) : base(repository)
        {
        }
    }
}
