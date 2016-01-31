using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Statuses
{
    public class StatusDetailViewModel : DetailViewModelBase<StatusModel>
    {
        protected override bool CanRemove()
        {
            return base.CanRemove() && Model.GameCount == 0;
        }
    }
}
