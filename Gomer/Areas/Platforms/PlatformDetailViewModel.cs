using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Platforms
{
    public class PlatformDetailViewModel : DetailViewModelBase<PlatformModel>
    {
        protected override bool CanRemove()
        {
            return base.CanRemove() && Model.GameCount == 0;
        }
    }
}
