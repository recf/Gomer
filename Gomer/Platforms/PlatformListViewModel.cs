using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Platforms
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
