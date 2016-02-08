using System.Collections.ObjectModel;
using Gomer.DataAccess;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Statuses
{
    public class StatusIndexViewModel : IndexViewModelBase<IStatusRepository, StatusModel, StatusListViewModel, StatusDetailViewModel>
    {
        public StatusIndexViewModel(IStatusRepository repository) :
            base(repository, new StatusListViewModel(repository), new StatusDetailViewModel())
        {
        }

        public override bool SupportsNew
        {
            get { return false; }
        }
    }
}
