using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Models;

namespace Gomer.DataAccess.Implementation
{
    public class PlatformRepository : RepositoryBase<PlatformModel>, IPlatformRepository
    {
        public PlatformRepository(IDataContext context) : base(context)
        {
        }

        protected override ISet<PlatformModel> Set
        {
            get { return Context.Platforms; }
        }

        protected override IEnumerable<PlatformModel> DefaultSort(IEnumerable<PlatformModel> models)
        {
            return models.OrderByDescending(x => x.GameCount).ThenBy(x => x.Name);
        }

        protected override PlatformModel PopulateSecondaryData(PlatformModel model)
        {
            model.GameCount = Context.Games.Count(x => x.Platforms.Select(p => p.Id).Contains(model.Id));

            return model;
        }
    }
}
