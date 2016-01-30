using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Models;

namespace Gomer.DataAccess.Implementation
{
    public class StatusRepository : RepositoryBase<StatusModel>, IStatusRepository
    {
        public StatusRepository(IDataContext context) : base(context)
        {
        }

        protected override ISet<StatusModel> Set
        {
            get { return Context.Statuses; }
        }

        protected override IEnumerable<StatusModel> DefaultSort(IEnumerable<StatusModel> models)
        {
            return models.OrderBy(x => x.Order);
        }
    }
}
