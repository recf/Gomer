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
    public class PlatformIndexViewModel: IndexViewModelBase<PlatformModel, PlatformListViewModel, PlatformDetailViewModel>
    {
        public PlatformIndexViewModel(ObservableCollection<PlatformModel> models) : 
            base(models, 
            new PlatformListViewModel(models), 
            new PlatformDetailViewModel())
        {
        }
    }
}
