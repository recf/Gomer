using System.Collections.ObjectModel;
using Gomer.DataAccess;
using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Platforms
{
    public class PlatformListViewModel : ListViewModelBase<IPlatformRepository, PlatformModel>
    {
        public PlatformListViewModel(IPlatformRepository repository) : base(repository)
        {
        }
    }
}
