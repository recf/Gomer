using System.Collections.ObjectModel;
using Gomer.DataAccess;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Platforms
{
    public class PlatformIndexViewModel: IndexViewModelBase<IPlatformRepository, PlatformModel, PlatformListViewModel, PlatformDetailViewModel>
    {
        public PlatformIndexViewModel(IPlatformRepository repository) : 
            base(repository, 
            new PlatformListViewModel(repository), 
            new PlatformDetailViewModel())
        {
        }
    }
}
