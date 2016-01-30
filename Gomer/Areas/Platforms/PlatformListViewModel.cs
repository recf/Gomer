using System.Collections.ObjectModel;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Platforms
{
    public class PlatformListViewModel : ListViewModelBase<PlatformModel>
    {
        public PlatformListViewModel(ObservableCollection<PlatformModel> models) : base(models)
        {
        }

        public PlatformListViewModel()
            : this(new ObservableCollection<PlatformModel>())
        {
        }
    }
}
