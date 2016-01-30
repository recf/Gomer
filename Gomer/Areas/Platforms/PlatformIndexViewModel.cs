using System.Collections.ObjectModel;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Platforms
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
