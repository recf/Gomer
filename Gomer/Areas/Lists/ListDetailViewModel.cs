using Gomer.Generic;
using Gomer.Models;

namespace Gomer.Areas.Lists
{
    public class ListDetailViewModel : DetailViewModelBase<ListModel>
    {
        protected override bool CanRemove()
        {
            return base.CanRemove() && Model.GameCount == 0;
        }
    }
}
